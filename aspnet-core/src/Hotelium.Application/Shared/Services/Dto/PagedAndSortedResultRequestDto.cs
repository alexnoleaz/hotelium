namespace Hotelium.Shared.Services.Dto;

public class PagedAndSortedResultRequestDto : PagedResultRequestDto, IPagedAndSortedResultRequest
{
    public virtual string Sorting { get; set; }
}