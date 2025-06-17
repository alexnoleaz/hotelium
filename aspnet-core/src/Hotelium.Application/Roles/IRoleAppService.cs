using Hotelium.Shared.Services;
using Hotelium.Roles.Dto;

namespace Hotelium.Roles;

public interface IRoleAppService : IAsyncCrudAppService<RoleDto, int, PagedRoleResultRequestDto, CreateRoleDto, RoleDto>
{
}