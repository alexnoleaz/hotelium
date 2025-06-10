namespace Hotelium.Shared.Services.Dto;

public class LimitedResultRequestDto : ILimitedResultRequest
{
    public static int DefaultMaxResultCount { get; set; } = 10;

    public virtual int MaxResultCount { get; set; } = DefaultMaxResultCount;
}