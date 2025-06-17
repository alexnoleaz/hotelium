using Hotelium.Shared.Entities;
using Hotelium.Shared.Entities.Auditing;
using Hotelium.Users;

namespace Hotelium.Roles;

public class Role : FullAuditedEntity, IPassivable
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<User> Users { get; set; } = new HashSet<User>();
}