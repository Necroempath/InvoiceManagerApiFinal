using AutoMapper;
using FluentValidation;
using InvoiceManagerApiFinal.DTOs;
using InvoiceManagerApiFinal.Models;
using InvoiceManagerApiFinal.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;

namespace InvoiceManagerApiFinal.Services.Implementations;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public UserService(UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<UserResponseDto> ChangePasswordAsync(ChangePasswordRequest request, ClaimsPrincipal principal)
    {
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            throw new UnauthorizedAccessException("Unauthorized access");

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if (!result.Succeeded)
        {
            var errors = result.Errors
                .GroupBy(e => e.Code)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.Description).ToArray()
                );

            throw new ValidationException(errors.SelectMany(kv =>
                kv.Value.Select(v => new FluentValidation.Results.ValidationFailure(kv.Key, v))
            ));
        }

        user.Password = request.NewPassword;

        await _userManager.UpdateAsync(user);

        return _mapper.Map<UserResponseDto>(user);
    }

    public async Task<UserResponseDto> ChangeProfileAsync(ChangeProfileRequest request, ClaimsPrincipal principal)
    {
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            throw new UnauthorizedAccessException("Unauthorized access");

        user.Name = request.Name;
        user.Address = request.Address;
        user.PhoneNumber = request.PhoneNumber;

        await _userManager.UpdateAsync(user);

        return _mapper.Map<UserResponseDto>(user);
    }
}
