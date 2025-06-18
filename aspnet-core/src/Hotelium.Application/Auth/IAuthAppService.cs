using Hotelium.Auth.Dto;
using Hotelium.Shared.Services;
using Hotelium.Users.Dto;

namespace Hotelium.Auth;

public interface IAuthAppService : IApplicationService
{
    Task<UserDto> Login(LoginRequestDto input);
    Task<UserDto> Register(CreateUserDto input);
}