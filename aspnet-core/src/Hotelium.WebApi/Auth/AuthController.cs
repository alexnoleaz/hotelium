using Hotelium.Users.Dto;
using Hotelium.Shared;
using Hotelium.Auth.Dto;
using Hotelium.Auth.JwtBearer;

namespace Hotelium.Auth;

[ApiController]
[Route("api/[controller]")]
[TypeFilter(typeof(AuthExceptionFilter))]
public class AuthController : ControllerBase
{
    private readonly IAuthAppService _service;
    private readonly JwtTokenService _tokenService;
    private readonly TokenAuthConfiguration _tokenConfiguration;

    public AuthController(IAuthAppService service, JwtTokenService tokenService, TokenAuthConfiguration tokenConfiguration)
    {
        _service = service;
        _tokenService = tokenService;
        _tokenConfiguration = tokenConfiguration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto input)
    {
        var result = await _service.Login(input);
        return Ok(CreateAuthResult(result));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto input)
    {
        var result = await _service.Register(input);
        return Ok(CreateAuthResult(result));
    }

    private Response<AuthResult> CreateAuthResult(UserDto user)
        => Response<AuthResult>.Success(new AuthResult
        {
            AccessToken = _tokenService.Generate(user.Id, user.RoleNames),
            UserId = user.Id,
            ExpireInSeconds = (int)_tokenConfiguration.Expiration.TotalSeconds
        });
}