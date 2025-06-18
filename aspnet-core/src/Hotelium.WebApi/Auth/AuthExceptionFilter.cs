using Hotelium.Shared;

namespace Hotelium.Auth;

public class AuthExceptionFilter : IExceptionFilter
{
    private readonly Shared.ILogger<AuthExceptionFilter> _logger;

    public AuthExceptionFilter(Shared.ILogger<AuthExceptionFilter> logger) => _logger = logger;

    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        var result = exception switch
        {
            InvalidCredentialsException ex => HandleInvalidCredentialsException(ex),
            _ => null
        };

        context.Result = result;
        context.ExceptionHandled = result is not null;
    }

    private IActionResult HandleInvalidCredentialsException(InvalidCredentialsException exception)
    {
        _logger.Warning(exception, exception.Message);

        var response = Response<string>.Fail(exception.Message);
        return new ObjectResult(response) { StatusCode = ((int)HttpStatusCode.BadRequest) };
    }
}