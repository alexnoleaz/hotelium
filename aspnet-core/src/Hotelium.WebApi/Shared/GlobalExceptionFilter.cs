using Hotelium.Shared.Exceptions;

namespace Hotelium.Shared;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger) => _logger = logger;

    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        var result = exception switch
        {
            EntityNotFoundException ex => HandleEntityNotFoundException(ex),
            ValueAlreadyUsedException ex => HandleValueAlreadyUsedException(ex),
            _ => HandleException(exception)
        };

        context.Result = result;
        context.ExceptionHandled = true;
    }

    private IActionResult HandleEntityNotFoundException(EntityNotFoundException exception)
    {
        _logger.Warning(exception, exception.Message);

        var response = Response<string>.Fail(exception.Message, HttpStatusCode.NotFound);
        return new ObjectResult(response) { StatusCode = ((int)HttpStatusCode.NotFound) };
    }

    private IActionResult HandleValueAlreadyUsedException(ValueAlreadyUsedException exception)
    {
        _logger.Warning(exception, exception.Message);

        var response = Response<string>.Fail(exception.Message, HttpStatusCode.Conflict);
        return new ObjectResult(response) { StatusCode = ((int)HttpStatusCode.Conflict) };
    }

    private IActionResult HandleException(Exception exception)
    {
        _logger.Error(exception, exception.Message);

        var response = Response.Error("An error occurred. Please contact support.");
        return new ObjectResult(response) { StatusCode = ((int)HttpStatusCode.InternalServerError) };
    }
}