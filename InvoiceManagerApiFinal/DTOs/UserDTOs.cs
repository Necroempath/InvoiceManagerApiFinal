namespace InvoiceManagerApiFinal.DTOs;

/// <summary>
/// DTO for changing a user's password. Uses for POST requests
/// </summary>
public class ChangePasswordRequest
{
    /// <summary>
    /// The user's current password. Required to verify identity before changing password.
    /// </summary>
    /// <example>Old!123</example>
    public string CurrentPassword { get; set; } = string.Empty;

    /// <summary>
    /// The new password the user wants to set. Must meet password policy requirements.
    /// </summary>
    /// <example>P@ss123</example>
    public string NewPassword { get; set; } = string.Empty;
}

/// <summary>
/// DTO for updating user's profile information. Uses for POST requests
/// </summary>
public class ChangeProfileRequest
{
    /// <summary>
    /// The user's full name. Cannot be empty.
    /// </summary>
    /// <example>John</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The user's postal or residential address. Optional.
    /// </summary>
    /// <example>Baku Nizami st. 41</example>
    public string? Address { get; set; }

    /// <summary>
    /// The user's phone number. Optional.
    /// </summary>
    /// <example>+994557818912</example>
    public string? PhoneNumber { get; set; }
}

/// <summary>
/// DTO for getting user's response.
/// </summary>
public class UserResponseDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
}