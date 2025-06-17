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
        var result = await _service.CreateAsync(input);
        var response = Response<RoleDto>.Success(result, HttpStatusCode.Created);

        return CreatedAtAction(nameof(Get), new { result.Id }, response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PagedRoleResultRequestDto input)
    {
        var result = await _service.GetAllAsync(input);
        var response = Response<PagedResultDto<RoleDto>>.Success(result);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _service.GetAsync(new EntityDto(id));
        var response = Response<RoleDto>.Success(result);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(new EntityDto(id));
        return Ok(Hotelium.Shared.Response.Success());
    }
}