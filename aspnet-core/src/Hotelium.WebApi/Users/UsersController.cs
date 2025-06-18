using Microsoft.AspNetCore.Authorization;
using Hotelium.Users.Dto;
using Hotelium.Shared.Services.Dto;
using Hotelium.Shared;
using Hotelium.Shared.Filters;

namespace Hotelium.Users;

[ApiController]
[Route("api/[controller]")]
[TypeFilter(typeof(UserExceptionFilter))]
public class UsersController : ControllerBase
{
    private readonly IUserAppService _service;

    public UsersController(IUserAppService service) => _service = service;

    [HttpPost]
    [Authorize(Roles = "Admin,Mod")]
    public async Task<IActionResult> Create([FromBody] CreateUserDto input)
    {
        var response = Response<UserDto>.Success(await _service.CreateAsync(input), HttpStatusCode.Created);
        return CreatedAtAction(nameof(Get), new { id = response.Data?.Id }, response);
    }

    [HttpGet("{id:long}")]
    [Authorize(Roles = "Admin,Mod,User")]
    public async Task<IActionResult> Get(long id)
        => Ok(Response<UserDto>.Success(await _service.GetAsync(new EntityDto<long>(id))));

    [HttpGet]
    [Authorize(Roles = "Admin,Mod,User")]
    public async Task<IActionResult> GetAll([FromQuery] PagedUserResultRequestDto input)
        => Ok(Response<PagedResultDto<UserDto>>.Success(await _service.GetAllAsync(input)));

    [HttpPut("{id:long}")]
    [ValidateRouteIdMatchesBodyId]
    [Authorize(Roles = "Admin,Mod,User")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateUserDto input)
        => Ok(Response<UserDto>.Success(await _service.UpdateAsync(input)));

    [HttpDelete("{id:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(long id)
    {
        await _service.DeleteAsync(new EntityDto<long>(id));
        return SuccessResponse();
    }

    [HttpPut("{id}/deactivate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeActivate(long id)
    {
        await _service.DeActivate(new EntityDto<long>(id));
        return SuccessResponse();
    }

    [HttpPut("{id}/activate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Activate(long id)
    {
        await _service.Activate(new EntityDto<long>(id));
        return SuccessResponse();
    }

    [HttpPut("{id}/change-password")]
    [Authorize(Roles = "Admin,Mod,User")]
    public async Task<IActionResult> ChangePassword(long id, [FromBody] ChangePasswordDto input)
    {
        await _service.ChangePassword(id, input);
        return SuccessResponse();
    }

    private IActionResult SuccessResponse() => Ok(Hotelium.Shared.Response.Success());
}