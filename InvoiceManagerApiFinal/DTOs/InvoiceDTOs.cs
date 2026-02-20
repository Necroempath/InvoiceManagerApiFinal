using InvoiceManagerApi.Enums;

namespace InvoiceManagerApiFinal.DTOs;

/// <summary>
/// DTO for Invoice creation. Uses for GET requests
/// </summary>
public class InvoiceCreateRequest
{
    /// <summary>
    /// Start date of service
    /// </summary>
    /// <example>2026-02-04T12:54:27.393Z</example>
    public DateTimeOffset StartDate { get; set; }

    /// <summary>
    /// End date of service
    /// </summary>
    /// <example>2026-05-04T12:54:27.393Z</example>
    public DateTimeOffset EndDate { get; set; }

    /// <summary>
    /// Invoice comment
    /// </summary>
    /// <example>Your comment</example>
    public string? Comment { get; set; }

    /// <summary>
    /// Customer ID
    /// </summary>
    /// <example>1</example>
    public int CustomerId { get; set; }
}

public class InvoiceQueryParams
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Sort { get; set; }
    public string? SortDirection { get; set; }
    public string? Search { get; set; }
    public string? Status { get; set; }
    public decimal? MinSum { get; set; }
    public decimal? MaxSum { get; set; }
    public int? CustomerId { get; set; }

    public void Validate()
    {
        if (Page < 1) Page = 1;

        if (PageSize < 1) PageSize = 1;

        if (PageSize > 100) PageSize = 100;

        if (string.IsNullOrWhiteSpace(SortDirection)) SortDirection = "asc";

        if (MinSum < 0) MinSum = 0;

        if (MaxSum < 0) MaxSum = 0;

        if (MinSum > MaxSum) (MinSum, MaxSum) = (MaxSum, MinSum);

        SortDirection = SortDirection.ToLower();

        Search = Search?.ToLower();

        if (SortDirection != "asc" && SortDirection != "desc") SortDirection = "asc";
    }
}

/// <summary>
/// DTO for Invoice response. Used for GET requests.
/// </summary>
public class InvoiceResponseDto
{
    /// <summary>
    /// Unique identifier of the invoice
    /// </summary>
    /// <example>10</example>
    public int Id { get; set; }

    /// <summary>
    /// Start date of the service period
    /// </summary>
    /// <example>2026-02-04T12:54:27.393Z</example>
    public DateTimeOffset StartDate { get; set; }

    /// <summary>
    /// End date of the service period
    /// </summary>
    /// <example>2026-05-04T12:54:27.393Z</example>
    public DateTimeOffset EndDate { get; set; }

    /// <summary>
    /// Current invoice status
    /// </summary>
    /// <example>Paid</example>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Optional invoice comment
    /// </summary>
    /// <example>Monthly service payment</example>
    public string? Comment { get; set; }

    /// <summary>
    /// Total amount of the invoice
    /// </summary>
    /// <example>1250.75</example>
    public decimal TotalSum { get; set; }

    /// <summary>
    /// Number of invoice rows
    /// </summary>
    /// <example>3</example>
    public int RowsCount { get; set; }

    /// <summary>
    /// Identifier of the related customer
    /// </summary>
    /// <example>1</example>
    public int CustomerId { get; set; }

    /// <summary>
    /// Name of the customer
    /// </summary>
    /// <example>John Doe</example>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Email address of the customer
    /// </summary>
    /// <example>john.doe@example.com</example>
    public string CustomerEmail { get; set; } = string.Empty;
}

/// <summary>
/// DTO for invoice status modify. Uses for PATCH requests
/// </summary>
public class InvoiceStatusChangeRequest
{
    /// <summary>
    /// Invoice status
    /// </summary>
    /// <example>Sent</example>
    public InvoiceStatus Status { get; set; }
}

/// <summary>
/// DTO for Invoice update. Used for PUT requests.
/// </summary>
public class InvoiceUpdateRequest
{
    /// <summary>
    /// Updated start date of the service period
    /// </summary>
    /// <example>2026-02-04T12:54:27.393Z</example>
    public DateTimeOffset StartDate { get; set; }

    /// <summary>
    /// Updated end date of the service period
    /// </summary>
    /// <example>2026-05-04T12:54:27.393Z</example>
    public DateTimeOffset EndDate { get; set; }

    /// <summary>
    /// Updated invoice comment
    /// </summary>
    /// <example>Updated service period and pricing</example>
    public string? Comment { get; set; }
}