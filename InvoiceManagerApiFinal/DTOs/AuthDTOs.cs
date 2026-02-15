namespace InvoiceManagerApiFinal.DTOs;

/// <summary>
/// DTO for user registration. Uses for POST requests
/// </summary>
public class RegisterRequestDto
{
    /// <summary>
    /// User Name
    /// </summary>
    /// <example>John</example>
    public string Name { get; set; } = null!;

    /// <summary>
    /// User Email
    /// </summary>
    /// <example>your@email.com</example>
    public string Email { get; set; } = null!;

    /// <summary>
    /// User Password
    /// </summary>
    /// <example>yourpassword</example>
    public string Password { get; set; } = null!;

    /// <summary>
    /// User Confirm Password. Must be the same as Password
    /// </summary>
    /// <example>yourpassword</example>
    public string ConfirmPassword { get; set; } = null!;

    /// <summary>
    /// User Address
    /// </summary>
    /// <example>Baku, Nizami St. 12</example>
    public string? Address { get; set; }

    /// <summary>
    /// Customer Phone Number
    /// </summary>
    /// <example>+9946661135</example>
    public string? PhoneNumber { get; set; }
}

/// <summary>
/// DTO for user login. Uses for POST requests
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// User Email
    /// </summary>
    /// <example>your@email.com</example>
    public string Email { get; set; } = null!;

    /// <summary>
    /// User Password
    /// </summary>
    /// <example>yourpassword</example>
    public string Password { get; set; } = null!;
}

/// <summary>
/// DTO for user authentication response. Uses for POST requests
/// </summary>
public class AuthResponseDto
{
    public string Email { get; set; } = null!;
}
