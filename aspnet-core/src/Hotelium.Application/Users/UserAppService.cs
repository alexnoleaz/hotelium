using Hotelium.Roles;
using Hotelium.Shared;
using Hotelium.Shared.Exceptions;
using Hotelium.Shared.Linq;
using Hotelium.Shared.Repositories;
using Hotelium.Shared.Services;
using Hotelium.Shared.Services.Dto;
using Hotelium.Users.Dto;

namespace Hotelium.Users;

public class UserAppService : AsyncCrudAppService<UserAppService, User, UserDto, long, PagedUserResultRequestDto, CreateUserDto, UpdateUserDto>, IUserAppService
{
    private readonly IRepository<Role> _roleRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserAppService(
            IRepository<Role> roleRepository,
        IPasswordHasher passwordHasher,
        IAsyncQueryableExecuter asyncQueryableExecuter,
        IRepository<User, long> repository,
        ILogger<UserAppService> logger,
        IObjectMapper objectMapper
    )
        : base(
            asyncQueryableExecuter,
            repository,
            logger,
            objectMapper
        )
    {
        _roleRepository = roleRepository;
        _passwordHasher = passwordHasher;
    }

    public override async Task<UserDto> CreateAsync(CreateUserDto input)
    {
        await ValidateEmailIsUnique(input.Email);

        var user = MapToEntity(input);
        user.Password = _passwordHasher.HashPassword(input.Password);

        await SetUserRolesAsync(user, input.RoleNames);
        await Repository.InsertAsync(user);
        await Repository.SaveChangesAsync();

        return MapToEntityDto(user);
    }

    public override async Task<UserDto> UpdateAsync(UpdateUserDto input)
    {
        await ValidateEmailIsUnique(input.Email, input.Id);

        var user = await GetEntityByIdAsync(input.Id);
        MapToEntity(input, user);

        await SetUserRolesAsync(user, input.RoleNames, true);
        await Repository.UpdateAsync(user);
        await Repository.SaveChangesAsync();

        return MapToEntityDto(user);
    }

    public async Task DeActivate(EntityDto<long> input)
    {
        await Repository.UpdateAsync(input.Id, async (entity) => entity.IsActive = false);
        await Repository.SaveChangesAsync();
    }

    public async Task Activate(EntityDto<long> input)
    {
        await Repository.UpdateAsync(input.Id, async (entity) => entity.IsActive = true);
        await Repository.SaveChangesAsync();
    }

    public async Task ChangePassword(long id, ChangePasswordDto input)
    {
        var dbUser = await base.GetEntityByIdAsync(id);
        if (dbUser.Password != input.CurrentPassword)
            throw new InvalidCurrentPasswordException();

        dbUser.Password = _passwordHasher.HashPassword(input.NewPassword);
        await Repository.UpdateAsync(dbUser);
        await Repository.SaveChangesAsync();
    }

    public override async Task DeleteAsync(EntityDto<long> input)
    {
        var user = await GetEntityByIdAsync(input.Id);
        user.Roles.Clear();

        await Repository.UpdateAsync(user);
        await Repository.SaveChangesAsync();

        await Repository.DeleteAsync(user);
        await Repository.SaveChangesAsync();
    }

    protected override UserDto MapToEntityDto(User entity)
    {
        var user = base.MapToEntityDto(entity);
        user.RoleNames = entity.Roles.Select(r => r.Name).ToArray();

        return user;
    }

    protected override async Task<User> GetEntityByIdAsync(long id)
        => await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAllIncluding(u => u.Roles).Where(u => u.Id == id))
            ?? throw new EntityNotFoundException(typeof(User), id);

    protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedUserResultRequestDto input)
        => query.OrderBy(u => u.Email);

    protected override IQueryable<User> CreateFilteredQuery(PagedUserResultRequestDto input)
        => Repository.GetAllIncluding(u => u.Roles)
            .WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), u =>
                    u.Name.Contains(input.Keyword!) ||
                    u.Email.Contains(input.Keyword!))
            .WhereIf(input.IsActive.HasValue, u => u.IsActive == input.IsActive);

    private async Task<List<Role>> GetRolesByName(IEnumerable<string> roleNames)
        => await AsyncQueryableExecuter.ToListAsync(_roleRepository.GetAll().Where(r => roleNames.Contains(r.Name)));

    private async Task ValidateEmailIsUnique(string email, long? excludedUserId = null)
    {
        var existingUser = await Repository.LongCountAsync(u => u.Email == email &&
                (excludedUserId == null || u.Id != excludedUserId));
        if (existingUser > 0)
            throw new ValueAlreadyUsedException(nameof(email), email);
    }

    private async Task SetUserRolesAsync(User user, string[]? roleNames, bool isUpdate = false)
    {
        if (isUpdate)
            user.Roles.Clear();

        if (roleNames is null || roleNames.Length == 0)
            return;

        var roles = await GetRolesByName(roleNames);
        foreach (var role in roles)
            user.Roles.Add(role);
    }
}