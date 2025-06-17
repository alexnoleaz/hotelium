using Hotelium.Shared.Services.Dto;

namespace Hotelium.Users.Dto;

public class PagedUserResultRequestDto : PagedResultRequestDto
{
    public string? Keyword { get; set; }

    public bool? IsActive { get; set; }
}