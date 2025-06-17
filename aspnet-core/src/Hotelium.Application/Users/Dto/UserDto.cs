using Hotelium.Shared.Services.Dto;

namespace Hotelium.Users.Dto;

public class UserDto : CreationAuditedEntityDto<long>
{
    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public bool IsActive { get; set; }

    public string[] RoleNames { get; set; } = null!;
}