using AutoMapper;
using InvoiceManagerApiFinal.DTOs;
using InvoiceManagerApiFinal.Models;
using InvoiceManagerApiFinal.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace InvoiceManagerApiFinal.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public AuthService(UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null) return null;

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordValid) return null;

        return _mapper.Map<AuthResponseDto>(user);
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto request)
    {
        var isUserExists = await _userManager
            .FindByEmailAsync(request.Email);

        if (isUserExists is not null) return null;

        var user = _mapper.Map<User>(request);

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded) return null;

        return _mapper.Map<AuthResponseDto>(user);
    }
}
