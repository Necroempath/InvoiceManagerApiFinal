using InvoiceManagerApi.Common;
using InvoiceManagerApi.DTOs.CustomerDTOs;
using InvoiceManagerApi.Models;

namespace InvoiceManagerApi.Services.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerResponseDto>> GetAllAsync();
	Task<CustomerResponseDto?> GetByIdAsync(int id);
	Task<PagedResult<CustomerResponseDto>> GetPagedResultAsync(CustomerQueryParams queryParams);
	Task<CustomerResponseDto?> CreateAsync(CustomerCreateRequest request);
    Task<CustomerResponseDto?> UpdateAsync(int id, CustomerUpdateRequest request);
	Task<bool> DeleteSoftAsync(int id);
	Task<bool> DeleteHardAsync(int id);
}