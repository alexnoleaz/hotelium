using System.Text.Json.Serialization;

namespace Hotelium.Shared;

public record Response
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; init; }

    public string Status { get; init; } = null!;
    public HttpStatusCode Code { get; init; }
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    public static Response Error(string message, HttpStatusCode code = HttpStatusCode.InternalServerError)
        => new()
        {
            Status = "error",
            Message = message,
            Code = code,
        };

    public static Response Success(HttpStatusCode code = HttpStatusCode.OK)
        => new()
        {
            Status = "success",
            Message = default,
            Code = code,
        };

    protected Response() { }
}

public sealed record Response<T> : Response
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data { get; init; }

    public static Response<T> Success(T data, HttpStatusCode code = HttpStatusCode.OK)
        => new()
        {
            Status = "success",
            Data = data,
            Message = default,
            Code = code,
        };

    public static Response<T> Fail(T data, HttpStatusCode code = HttpStatusCode.BadRequest)
        => new()
        {
            Status = "fail",
            Data = data,
            Message = default,
            Code = code,
        };

    private Response() { }
}