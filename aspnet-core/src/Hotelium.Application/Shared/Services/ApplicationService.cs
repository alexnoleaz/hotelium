namespace Hotelium.Shared.Services;

public abstract class ApplicationService<TCategoryName> : IApplicationService
{
    protected readonly ILogger<TCategoryName> Logger;
    protected readonly IObjectMapper ObjectMapper;

    protected ApplicationService(ILogger<TCategoryName> logger, IObjectMapper objectMapper)
    {
        Logger = logger;
        ObjectMapper = objectMapper;
    }
}