using InvoiceManagerApi.DTOs.InvoiceRowDTOs;

namespace InvoiceManagerApi.Services.Interfaces;

public interface IInvoiceRowService
{
    Task<IEnumerable<InvoiceRowResponseDto>> GetAllAsync();
    Task<InvoiceRowResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<InvoiceRowResponseDto>> GetByInvoiceIdAsync(int invoiceId);
    Task<InvoiceRowResponseDto?> CreateAsync(InvoiceRowCreateRequest request);
    Task<InvoiceRowResponseDto?> UpdateAsync(int id, InvoiceRowUpdateRequest request);
    Task<bool> DeleteHardAsync(int id);
}
