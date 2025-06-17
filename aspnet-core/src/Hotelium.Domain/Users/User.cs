using Hotelium.Shared.Entities.Auditing;
using Hotelium.Shared.Entities;
using Hotelium.Users.Values;
using Hotelium.Roles;

namespace Hotelium.Users;

public class User : FullAuditedEntity<long>, IPassivable
{
    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Phone { get; set; }

    public Address? Address { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<Role> Roles { get; set; } = new HashSet<Role>();
}