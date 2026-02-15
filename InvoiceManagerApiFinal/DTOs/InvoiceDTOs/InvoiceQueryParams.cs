namespace InvoiceManagerApi.DTOs.InvoiceDTOs;

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
