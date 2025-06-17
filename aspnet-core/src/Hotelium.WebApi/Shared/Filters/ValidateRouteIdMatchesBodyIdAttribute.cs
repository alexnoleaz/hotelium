namespace Hotelium.Shared.Filters;

public class ValidateRouteIdMatchesBodyIdAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var routeParam = context.ActionArguments
            .FirstOrDefault(arg => arg.Value is not null && IsSimpleIdType(arg.Value));

        var bodyArg = context.ActionArguments.Values
            .FirstOrDefault(arg => arg?.GetType().GetProperty("Id") != null);

        if (routeParam.Value is null || bodyArg is null)
            return;

        var bodyIdProp = bodyArg.GetType().GetProperty("Id");
        var bodyId = bodyIdProp?.GetValue(bodyArg);

        if (bodyId is null || !IdsAreEqual(routeParam.Value, bodyId))
            context.Result = new BadRequestObjectResult(Response<string>.Fail("ID mismatch between URL and body."));
    }

    private static bool IsSimpleIdType(object value)
        => value is int or long or Guid or string;

    private bool IdsAreEqual(object routeId, object bodyId)
        => routeId.ToString()?.Equals(bodyId.ToString(), StringComparison.OrdinalIgnoreCase) == true;
}