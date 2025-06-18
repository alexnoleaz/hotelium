namespace Hotelium.Auth;

public class AuthResult
{
    public string AccessToken { get; set; } = null!;

    public long UserId { get; set; }

    public int ExpireInSeconds { get; set; }
}