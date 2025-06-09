namespace Hotelium.Shared.Entities;

public class EntityNotFoundException : Exception
{
    private const string DefaultMessage = "The requested entity was not found.";

    public Type? EntityType { get; }
    public object? Id { get; }

    public EntityNotFoundException()
        : base(DefaultMessage) { }

    public EntityNotFoundException(string? message)
        : base(message) { }

    public EntityNotFoundException(string? message, Exception? innerException)
        : base(message, innerException) { }

    public EntityNotFoundException(Type? entityType, object? id)
        : this(entityType, id, null) { }

    public EntityNotFoundException(Type? entityType, object? id, Exception? innerException)
        : base(
            $"Entity of type '{entityType?.FullName}' with ID '{id}' was not found.",
            innerException
        )
    {
        EntityType = entityType;
        Id = id;
    }
}