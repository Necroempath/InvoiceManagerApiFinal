using InvoiceManagerApiFinal.DTOs;

namespace InvoiceManagerApiFinal.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
}
