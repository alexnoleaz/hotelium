
using Hotelium.Auth.Dto;
using Hotelium.Shared;
using Hotelium.Shared.Linq;
using Hotelium.Shared.Repositories;
using Hotelium.Shared.Services;
using Hotelium.Users;
using Hotelium.Users.Dto;

namespace Hotelium.Auth;

public class AuthAppService : ApplicationService<AuthAppService>, IAuthAppService
{
    private readonly IRepository<User, long> _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserAppService _userAppService;
    private readonly IAsyncQueryableExecuter _asyncQueryableExecuter;

    public AuthAppService(
        IAsyncQueryableExecuter asyncQueryableExecuter,
        IUserAppService userAppService,
        IRepository<User, long> userRepository,
        IPasswordHasher passwordHasher,
        ILogger<AuthAppService> logger,
        IObjectMapper objectMapper
    )
        : base(logger, objectMapper)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _userAppService = userAppService;
        _asyncQueryableExecuter = asyncQueryableExecuter;
    }

    public async Task<UserDto> Login(LoginRequestDto input)
    {
        var dbUser = await _asyncQueryableExecuter.FirstOrDefaultAsync(_userRepository.GetAllIncluding(u => u.Roles).Where(u => u.Email == input.Email));
        if (dbUser is null)
            throw new InvalidCredentialsException();

        if (!_passwordHasher.VerifyHashedPassword(dbUser.Password, input.Password))
            throw new InvalidCredentialsException();

        var user = ObjectMapper.Map<UserDto>(dbUser);
        user.RoleNames = dbUser.Roles.Select(r => r.Name).ToArray();

        return user;
    }

    public Task<UserDto> Register(CreateUserDto input)
        => _userAppService.CreateAsync(input);
}