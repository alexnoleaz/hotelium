using Hotelium.Shared;

namespace Hotelium.Users;

public class UserExceptionFilter : IExceptionFilter
{
    private readonly Shared.ILogger<UserExceptionFilter> _logger;

    public UserExceptionFilter(Shared.ILogger<UserExceptionFilter> logger) => _logger = logger;

    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        var result = exception switch
        {
            InvalidCurrentPasswordException ex => HandleInvalidCurrentPasswordException(ex),
            _ => null
        };

        context.Result = result;
        context.ExceptionHandled = result is not null;
    }

    private IActionResult HandleInvalidCurrentPasswordException(InvalidCurrentPasswordException exception)
    {
        _logger.Warning(exception, exception.Message);

        var response = Response<string>.Fail(exception.Message);
        return new ObjectResult(response) { StatusCode = ((int)HttpStatusCode.BadRequest) };
    }
}