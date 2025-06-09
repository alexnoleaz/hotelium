using Hotelium.Shared.Dependency;

namespace Hotelium.Shared;

public class BcryptPasswordHasher : IPasswordHasher, ISingletonDependency
{
    public string HashPassword(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);

    public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        => BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
}