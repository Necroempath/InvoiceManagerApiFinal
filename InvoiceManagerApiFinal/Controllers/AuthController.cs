using InvoiceManagerApi.Common;
using InvoiceManagerApiFinal.DTOs;
using InvoiceManagerApiFinal.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceManagerApiFinal.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registers a new user with the provided registration details.
    /// </summary>
    /// <remarks>
    /// Returns user's email
    /// </remarks>
    /// <returns>
    /// Email of user wrapped in ApiResponse.
    /// </returns>
    /// <response code="200">User was successfully logged in.</response>
    /// <response code="401">Invalid email or password.</response>
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody]LoginRequestDto request)
    {
        var response = await _authService.LoginAsync(request);

        if (response is null) return Unauthorized(new ApiResponse<AuthResponseDto>
        {
            Success = false,
            Message = "Invalid email or password"
        });

        return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(response));
    }

    /// <summary>
    /// Login a user with the provided authentication details.
    /// </summary>
    /// <remarks>
    /// Returns user's email
    /// </remarks>
    /// <returns>
    /// Email of user wrapped in ApiResponse.
    /// </returns>
    /// <response code="200">New User account was successfully created.</response>
    /// <response code="401">User with such email already exists</response>
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register([FromBody] RegisterRequestDto request)
    {
        var response = await _authService.RegisterAsync(request);

        if (response is null) return Unauthorized(new ApiResponse<AuthResponseDto>
        {
            Success = false,
            Message = "User with such email already exists"
        });

        return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(response));
    }
}
