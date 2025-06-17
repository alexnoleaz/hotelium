using Hotelium.Shared.Services.Dto;
using Hotelium.Roles.Dto;
using Hotelium.Shared;

namespace Hotelium.Roles;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleAppService _service;

    public RolesController(IRoleAppService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleDto input)
    {
        var response = Response<RoleDto>.Success(await _service.CreateAsync(input), HttpStatusCode.Created);
        return CreatedAtAction(nameof(Get), new { response.Data?.Id }, response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PagedRoleResultRequestDto input)
        => Ok(Response<PagedResultDto<RoleDto>>.Success(await _service.GetAllAsync(input)));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
        => Ok(Response<RoleDto>.Success(await _service.GetAsync(new EntityDto(id))));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(new EntityDto(id));
        return Ok(Hotelium.Shared.Response.Success());
    }
}