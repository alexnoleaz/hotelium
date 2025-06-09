using Microsoft.Extensions.Logging;

using MsLogging = Microsoft.Extensions.Logging;

namespace Hotelium.Shared;

public class MicrosoftLogger<TCategoryName>(MsLogging.ILogger<TCategoryName> logger) : ILogger<TCategoryName>
{
    private readonly MsLogging.ILogger<TCategoryName> _logger = logger;

    public void Debug(Exception? exception, string? message, params object?[] args) =>
        _logger.LogDebug(exception, message, args);

    public void Debug(string? message, params object?[] args) => _logger.LogDebug(message, args);

    public void Error(Exception? exception, string? message, params object?[] args) =>
        _logger.LogError(exception, message, args);

    public void Error(string? message, params object?[] args) => _logger.LogError(message, args);

    public void Information(Exception? exception, string? message, params object?[] args) =>
        _logger.LogInformation(exception, message, args);

    public void Information(string? message, params object?[] args) =>
        _logger.LogInformation(message, args);

    public void Warning(Exception? exception, string? message, params object?[] args) =>
        _logger.LogWarning(exception, message, args);

    public void Warning(string? message, params object?[] args) =>
        _logger.LogWarning(message, args);
}