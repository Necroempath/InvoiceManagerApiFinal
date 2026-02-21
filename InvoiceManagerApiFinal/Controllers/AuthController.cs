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
    /// Returns user's email, refresh and access tokens
    /// </remarks>
    /// <returns>
    /// Email, refresh and access tokens of user wrapped in ApiResponse.
    /// </returns>
    /// <response code="200">User was successfully logged in.</response>
    /// <response code="401">Invalid email or password.</response>
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody]LoginRequestDto request)
    {
        var response = await _authService.LoginAsync(request);

        return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(response, "User login was successful"));
    }

    /// <summary>
    /// Login a user with the provided authentication details.
    /// </summary>
    /// <remarks>
    /// Returns user's email, refresh and access tokens
    /// </remarks>
    /// <returns>
    /// Email, refresh and access tokens of user wrapped in ApiResponse.
    /// </returns>
    /// <response code="200">New User account was successfully created.</response>
    /// <response code="401">User with such email already exists</response>
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register([FromBody] RegisterRequestDto request)
    {
        var response = await _authService.RegisterAsync(request);

        return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(response, "New User has registered"));
    }

    /// <summary>
    /// Refresh both refresh and access tokens if they prove valid.
    /// </summary>
    /// <remarks>
    /// Returns user's email, refresh and access tokens
    /// </remarks>
    /// <returns>
    /// Email, refresh and access tokens of user wrapped in ApiResponse.
    /// </returns>
    /// <response code="200">Tokens were refreshed.</response>
    /// <response code="401">Invalid token exception</response>
    [HttpPost("refresh")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Refresh([FromBody] RefreshTokenRequest request)
    {
        var response = await _authService.RefreshAsync(request);

        return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(response, "Tokens refreshed"));
    }
}
