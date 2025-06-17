using Hotelium.Shared.Services;
using Hotelium.Shared.Services.Dto;
using Hotelium.Users.Dto;

namespace Hotelium.Users;

public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UpdateUserDto>
{
    Task DeActivate(EntityDto<long> input);
    Task Activate(EntityDto<long> input);
    Task ChangePassword(long id, ChangePasswordDto input);
}