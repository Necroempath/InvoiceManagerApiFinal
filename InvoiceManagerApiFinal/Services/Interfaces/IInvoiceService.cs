using InvoiceManagerApi.Common;
using InvoiceManagerApi.DTOs.InvoiceDTOs;

namespace InvoiceManagerApi.Services.Interfaces;

public interface IInvoiceService
{
    Task<IEnumerable<InvoiceResponseDto>> GetAllAsync();
    Task<InvoiceResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<InvoiceResponseDto>> GetByCustomerIdAsync(int customerId);
    Task<PagedResult<InvoiceResponseDto>> GetPagedResultAsync(InvoiceQueryParams queryParams);
    Task<InvoiceResponseDto?> CreateAsync(InvoiceCreateRequest request);
    Task<InvoiceResponseDto?> UpdateAsync(int id, InvoiceUpdateRequest request);
    Task<InvoiceResponseDto?> StatusChangeAsync(int id, InvoiceStatusChangeRequest status);
    Task<bool> DeleteSoftAsync(int id);
    Task<bool> DeleteHardAsync(int id);
}
