namespace Hotelium.Shared;

public interface ILogger<TCategoryName>
{
    void Debug(Exception? exception, string? message, params object?[] args);

    void Debug(string? message, params object?[] args);

    void Error(Exception? exception, string? message, params object?[] args);

    void Error(string? message, params object?[] args);

    void Information(Exception? exception, string? message, params object?[] args);

    void Information(string? message, params object?[] args);

    void Warning(Exception? exception, string? message, params object?[] args);

    void Warning(string? message, params object?[] args);
}