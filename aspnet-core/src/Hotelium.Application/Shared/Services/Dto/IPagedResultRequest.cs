namespace Hotelium.Shared.Services.Dto;

public interface IPagedResultRequest : ILimitedResultRequest
{
    int SkipCount { get; set; }
}