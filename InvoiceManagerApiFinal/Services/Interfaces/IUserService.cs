using InvoiceManagerApiFinal.DTOs;
using InvoiceManagerApiFinal.Models;
using System.Security.Claims;

namespace InvoiceManagerApiFinal.Services.Interfaces;

public interface IUserService
{
    Task<UserResponseDto> ChangePasswordAsync(ChangePasswordRequest request, ClaimsPrincipal principal);    
    Task<UserResponseDto> ChangeProfileAsync(ChangeProfileRequest request, ClaimsPrincipal principal);    
}
