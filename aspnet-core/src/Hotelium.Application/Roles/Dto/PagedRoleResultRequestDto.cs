using Hotelium.Shared.Services.Dto;

namespace Hotelium.Roles.Dto;

public class PagedRoleResultRequestDto : PagedResultRequestDto
{
    public string? Keyword { get; set; }
}