namespace Hotelium.Shared.Exceptions;

public class ValueAlreadyUsedException : Exception
{
    private const string DefaultMessage = "The value is already used.";

    public string? Property { get; }
    public object? Value { get; }

    public ValueAlreadyUsedException()
        : base(DefaultMessage) { }

    public ValueAlreadyUsedException(string? message)
        : base(message) { }

    public ValueAlreadyUsedException(string? message, Exception? innerException)
        : base(message, innerException) { }

    public ValueAlreadyUsedException(string? property, object? value)
        : this(property, value, null) { }

    public ValueAlreadyUsedException(string? property, object? value, Exception? innerException)
        : base(
            $"The value '{value}' of property '{property}' is already used.",
            innerException
        )
    {
        Property = property;
        Value = value;
    }
}