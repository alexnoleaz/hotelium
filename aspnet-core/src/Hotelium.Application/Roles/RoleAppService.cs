using Hotelium.Roles.Dto;
using Hotelium.Shared;
using Hotelium.Shared.Exceptions;
using Hotelium.Shared.Linq;
using Hotelium.Shared.Repositories;
using Hotelium.Shared.Services;
using Hotelium.Shared.Services.Dto;

namespace Hotelium.Roles;

public class RoleAppService : AsyncCrudAppService<RoleAppService, Role, RoleDto, int, PagedRoleResultRequestDto, CreateRoleDto, RoleDto>, IRoleAppService
{
    public RoleAppService(
        IAsyncQueryableExecuter asyncQueryableExecuter,
        IRepository<Role, int> repository,
        ILogger<RoleAppService> logger,
        IObjectMapper objectMapper
    )
        : base(
            asyncQueryableExecuter,
            repository,
            logger,
            objectMapper
        )
    { }

    public override async Task<RoleDto> CreateAsync(CreateRoleDto input)
    {
        var count = await Repository.CountAsync(x => x.Name == input.Name);
        if (count > 0)
            throw new ValueAlreadyUsedException(nameof(input.Name), input.Name);

        var role = MapToEntity(input);
        await Repository.InsertAsync(role);
        await Repository.SaveChangesAsync();

        return MapToEntityDto(role);
    }

    public override async Task DeleteAsync(EntityDto<int> input)
    {
        var role = await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAllIncluding(r => r.Users).Where(r => r.Id == input.Id))
            ?? throw new EntityNotFoundException(typeof(Role), input.Id);

        role.Users.Clear();

        await Repository.UpdateAsync(role);
        await Repository.SaveChangesAsync();

        await Repository.DeleteAsync(role);
        await Repository.SaveChangesAsync();
    }

    protected override IQueryable<Role> ApplySorting(IQueryable<Role> query, PagedRoleResultRequestDto input)
        => query.OrderBy(u => u.Name);

    protected override IQueryable<Role> CreateFilteredQuery(PagedRoleResultRequestDto input)
        => Repository.GetAll()
            .WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), r => r.Name.Contains(input.Keyword!));
}