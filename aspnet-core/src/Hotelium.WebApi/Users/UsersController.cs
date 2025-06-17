using Hotelium.Users.Dto;
using Hotelium.Shared.Services.Dto;
using Hotelium.Shared;

namespace Hotelium.Users;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserAppService _service;

    public UsersController(IUserAppService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto input)
    {
        var response = Response<UserDto>.Success(await _service.CreateAsync(input));
        return CreatedAtAction(nameof(Get), new { id = response.Data?.Id }, response);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id)
        => Ok(Response<UserDto>.Success(await _service.GetAsync(new EntityDto<long>(id))));

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PagedUserResultRequestDto input)
        => Ok(Response<PagedResultDto<UserDto>>.Success(await _service.GetAllAsync(input)));

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateUserDto input)
    {
        if (id != input.Id)
            return BadRequest("ID mismatch between URL and body");

        return Ok(Response<UserDto>.Success(await _service.UpdateAsync(input)));
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _service.DeleteAsync(new EntityDto<long>(id));
        return SuccessResponse();
    }

    [HttpPut("{id}/deactivate")]
    public async Task<IActionResult> DeActivate(long id)
    {
        await _service.DeActivate(new EntityDto<long>(id));
        return SuccessResponse();
    }

    [HttpPut("{id}/activate")]
    public async Task<IActionResult> Activate(long id)
    {
        await _service.Activate(new EntityDto<long>(id));
        return SuccessResponse();
    }

    [HttpPut("{id}/change-password")]
    public async Task<IActionResult> ChangePassword(long id, [FromBody] ChangePasswordDto input)
    {
        await _service.ChangePassword(id, input);
        return SuccessResponse();
    }

    private IActionResult SuccessResponse() => Ok(Hotelium.Shared.Response.Success());
}