using AutoMapper;
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

        return await GenerateTokenAsync(user);
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

        return await GenerateTokenAsync(user);
    }

    private async Task<AuthResponseDto> GenerateTokenAsync(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.SecretKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name , user.UserName!),
            new Claim(ClaimTypes.Email , user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config.Issuer,
            audience: _config.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_config.ExpirationInMinutes),
            signingCredentials: credentials
            );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        //var (refreshToken, refreshJwt) =
        //        await CreateRefreshTokenJwtAsync(user.Id, _config.RefreshTokenExpirationInDays);
        return new AuthResponseDto
        {
            Email = user.Email!,
            AccessToken = tokenString,
            ExpiredAt = DateTime.UtcNow.AddMinutes(_config.ExpirationInMinutes),
            //RefreshToken = refreshJwt,
            //RefreshTokenExpiredAt = refreshToken.ExpiresAt,
            //Roles = roles
        };
    }
}
