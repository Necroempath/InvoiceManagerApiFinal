namespace InvoiceManagerApiFinal.DTOs;

/// <summary>
/// DTO for Customer creation. Uses for POST requests
/// </summary>
public class CustomerCreateRequest
{
    /// <summary>
    /// Customer Name
    /// </summary>
    /// <example>John</example>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Customer Email
    /// </summary>
    /// <example>your@email.com</example>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Customer Address
    /// </summary>
    /// <example>Baku, Nizami St. 12</example>
    public string? Address { get; set; }

    /// <summary>
    /// Customer Phone Number
    /// </summary>
    /// <example>+9946661135</example>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Id of User Customer assigned to
    /// </summary>
    /// <example>1</example>
    public string UserId { get; set; } = null!;
}

public class CustomerQueryParams
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Sort { get; set; }
    public string? SortDirection { get; set; }
    public string? Search { get; set; }

    public void Validate()
    {
        if (Page < 1) Page = 1;

        if (PageSize < 1) PageSize = 1;

        if (PageSize > 100) PageSize = 100;

        if (string.IsNullOrWhiteSpace(SortDirection)) SortDirection = "asc";

        SortDirection = SortDirection.ToLower();

        Search = Search?.ToLower();

        if (SortDirection != "asc" && SortDirection != "desc") SortDirection = "asc";
    }
}

/// <summary>
/// DTO for returned Customer data. Uses for GET requests
/// </summary>
public class CustomerResponseDto()
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public int InvoiceCount { get; set; }
    public decimal? InvoicesSum { get; set; }
    public string UserId { get; set; } = null!;
}

/// <summary>
/// DTO for updating Customer. Uses for PUT request 
/// </summary>
public class CustomerUpdateRequest
{
    /// <summary>
    /// Customer Name
    /// </summary>
    /// <example>John</example>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Customer Email
    /// </summary>
    /// <example>your@email.com</example>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Customer Address
    /// </summary>
    /// <example>Baku, Nizami St. 12</example>
    public string? Address { get; set; }

    /// <summary>
    /// Customer Phone Number
    /// </summary>
    /// <example>+9946661135</example>
    public string? PhoneNumber { get; set; }
}