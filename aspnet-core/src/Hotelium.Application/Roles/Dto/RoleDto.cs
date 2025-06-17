using Hotelium.Shared.Services.Dto;

namespace Hotelium.Roles.Dto;

public class RoleDto : EntityDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}