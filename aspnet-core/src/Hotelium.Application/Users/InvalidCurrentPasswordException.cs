using Hotelium.Shared.Exceptions;

namespace Hotelium.Users;

public class InvalidCurrentPasswordException : DomainException
{
    public InvalidCurrentPasswordException() : base("The current password is incorrect.") { }
}