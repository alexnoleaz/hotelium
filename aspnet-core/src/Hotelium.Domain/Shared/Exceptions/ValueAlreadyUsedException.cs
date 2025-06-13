namespace Hotelium.Shared.Exceptions;

public class ValueAlreadyUsedException : Exception
{
    private const string DefaultMessage = "The value is already used.";

    public string? PropertyName { get; }
    public object? Value { get; }

    public ValueAlreadyUsedException()
        : base(DefaultMessage) { }

    public ValueAlreadyUsedException(string? message)
        : base(message) { }

    public ValueAlreadyUsedException(string? message, Exception? innerException)
        : base(message, innerException) { }

    public ValueAlreadyUsedException(string? propertyName, object? value)
        : this(propertyName, value, null) { }

    public ValueAlreadyUsedException(string? propertyName, object? value, Exception? innerException)
        : base(
            $"The value '{value}' of property '{propertyName}' is already used.",
            innerException
        )
    {
        PropertyName = propertyName;
        Value = value;
    }
}