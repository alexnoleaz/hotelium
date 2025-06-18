namespace Hotelium.Auth;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException() : base("Invalid credentials.") { }
}