namespace Hotelium.Shared.Services.Dto;

public class PagedResultRequestDto : LimitedResultRequestDto, IPagedResultRequest
{
    public virtual int SkipCount { get; set; }
}