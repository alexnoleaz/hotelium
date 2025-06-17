using AutoMapper;

namespace Hotelium.Roles.Dto;

public class RoleMapProfile : Profile
{
    public RoleMapProfile()
    {
        CreateMap<CreateRoleDto, Role>();
        CreateMap<Role, RoleDto>();
    }
}