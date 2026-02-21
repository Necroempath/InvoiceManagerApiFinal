using InvoiceManagerApi.Common;
using InvoiceManagerApiFinal.DTOs;
using InvoiceManagerApiFinal.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceManagerApiFinal.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userManager)
    {
        _userService = userManager;
    }

    [HttpPatch("password")]
    public async Task<ActionResult<ApiResponse<UserResponseDto>>> ChangePassword([FromBody]ChangePasswordRequest request)
    {
        var response = await _userService.ChangePasswordAsync(request, HttpContext.User);

        return Ok(ApiResponse<UserResponseDto>.SuccessResponse(response, "Password has successfully changed"));
    }

    [HttpPatch("profile")]
    public async Task<ActionResult<ApiResponse<UserResponseDto>>> ChangeProfile([FromBody] ChangeProfileRequest request)
    {
        var response = await _userService.ChangeProfileAsync(request, HttpContext.User);

        return Ok(ApiResponse<UserResponseDto>.SuccessResponse(response, "Profile has successfully changed"));
    }
}
