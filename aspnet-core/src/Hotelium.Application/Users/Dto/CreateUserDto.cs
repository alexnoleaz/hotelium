namespace Hotelium.Users.Dto;

public class CreateUserDto
{
    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string[]? RoleNames { get; set; }

    public AddressDto? Address { get; set; }
}