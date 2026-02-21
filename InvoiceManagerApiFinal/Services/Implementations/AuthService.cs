using AutoMapper;
using Azure;
using InvoiceManagerApiFinal.Config;
using InvoiceManagerApiFinal.DTOs;
using InvoiceManagerApiFinal.Models;
using InvoiceManagerApiFinal.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InvoiceManagerApiFinal.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly JwtConfig _config;

    public AuthService(UserManager<User> userManager, IMapper mapper, IOptions<JwtConfig> config)
    {
        _userManager = userManager;
        _mapper = mapper;
        _config = config.Value;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
       
        if (user is null) throw new InvalidOperationException($"Incorrect email or password");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordValid) throw new InvalidOperationException($"Incorrect email or password");

        return await RefreshTokenAsync(user);
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        var user = _mapper.Map<User>(request);

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var builder = new StringBuilder();

            foreach (var error in result.Errors)
            {              
                builder.AppendLine(error.Description);
            }

            throw new InvalidOperationException(builder.ToString());
        }

        return await RefreshTokenAsync(user);
    }

    public async Task<AuthResponseDto> RefreshAsync(RefreshTokenRequest request)
    {
        var principal = GetPrincipalFromAccessToken(request.AcessToken);
        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await _userManager.FindByIdAsync(userId!);

        if (user is null)
            throw new InvalidOperationException("User not found");

        if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiresAt < DateTime.UtcNow)
            throw new UnauthorizedAccessException("Invalid refresh token");

        return await RefreshTokenAsync(user);
    }

    private ClaimsPrincipal GetPrincipalFromAccessToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            ValidIssuer = _config.Issuer,
            ValidAudience = _config.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config.SecretKey))
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }

    private async Task<AuthResponseDto> RefreshTokenAsync(User user)
    {
        var authResponseDto = await GenerateTokenAsync(user);

        user.RefreshToken = authResponseDto.RefreshToken;
        user.RefreshTokenExpiresAt = authResponseDto.RefreshTokenExpiresAt;

        await _userManager.UpdateAsync(user);

        return authResponseDto;
    }

    private async Task<AuthResponseDto> GenerateTokenAsync(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.SecretKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name , user.Name!),
            new(ClaimTypes.Email , user.Email!),
            new(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config.Issuer,
            audience: _config.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_config.ExpirationInMinutes),
            signingCredentials: credentials
            );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new AuthResponseDto
        {
            Email = user.Email!,
            AccessToken = tokenString,
            ExpiredAt = DateTime.UtcNow.AddMinutes(_config.ExpirationInMinutes),
            RefreshToken = Guid.NewGuid().ToString("N").ToLower(),
            RefreshTokenExpiresAt = DateTimeOffset.UtcNow.AddDays(_config.RefreshTokenExpirationInDays),
        };
    }
}
