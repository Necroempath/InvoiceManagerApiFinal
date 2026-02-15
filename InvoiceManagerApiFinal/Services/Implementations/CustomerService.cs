using AutoMapper;
using InvoiceManagerApi.Common;
using InvoiceManagerApi.Data;
using InvoiceManagerApi.DTOs.CustomerDTOs;
using InvoiceManagerApi.Models;
using InvoiceManagerApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManagerApi.Services.Implementations;

public class CustomerService : ICustomerService
{
    private readonly InvoiceManagerDbContext _context;
    private readonly IMapper _mapper;

    public CustomerService(InvoiceManagerDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CustomerResponseDto?> CreateAsync(CustomerCreateRequest request)
    {
        var isUserExists = await _context.Users
            .AnyAsync(u => u.Id == request.UserId);

        if (!isUserExists) return null;            

        Customer customer = _mapper.Map<Customer>(request);

        await _context.Customers.AddAsync(customer);

        await _context.SaveChangesAsync();

        return _mapper.Map<CustomerResponseDto>(customer);
    }

    public async Task<IEnumerable<CustomerResponseDto>> GetAllAsync()
    {
        IEnumerable<Customer> customers = await _context.Customers
            .Where(c => c.DeletedAt == null)
            .Include(c => c.Invoices.Where(i => i.DeletedAt == null))
            .ThenInclude(i => i.Rows)
            .ToListAsync();

        return _mapper.Map<IEnumerable<CustomerResponseDto>>(customers);
    }

    public async Task<CustomerResponseDto?> GetByIdAsync(int id)
    {
        Customer? customer = await _context.Customers
            .Include(c => c.Invoices.Where(i => i.DeletedAt == null))
            .ThenInclude(i => i.Rows)
            .FirstOrDefaultAsync(c => c.DeletedAt == null && c.Id == id);

        if (customer is null) return null;

        return _mapper.Map<CustomerResponseDto>(customer);
    }

    public async Task<bool> DeleteHardAsync(int id)
    {
        Customer? customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.DeletedAt == null && c.Id == id && c.Invoices.Count() == 0);

        if (customer is null) return false;

        _context.Customers.Remove(customer);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteSoftAsync(int id)
    {
        Customer? customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.DeletedAt == null && c.Id == id);

        if (customer is null) return false;

        customer.DeletedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<CustomerResponseDto?> UpdateAsync(int id, CustomerUpdateRequest request)
    {
        Customer? customer = await _context.Customers
            .Include(c => c.Invoices)
            .FirstOrDefaultAsync(c => c.DeletedAt == null && c.Id == id);

        if (customer is null) return null;

       _mapper.Map(request, customer);

        await _context.SaveChangesAsync();

        return _mapper.Map<CustomerResponseDto>(customer);

    }

    public async Task<PagedResult<CustomerResponseDto>> GetPagedResultAsync(CustomerQueryParams queryParams)
    {
        queryParams.Validate();

        var query = _context.Customers
            .Where(c => c.DeletedAt == null)
            .Include(c => c.Invoices.Where(i => i.DeletedAt == null))
            .ThenInclude(i => i.Rows)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryParams.Search))
            query = query.Where(c => c.Name.ToLower().Contains(queryParams.Search) || c.Email.ToLower().Contains(queryParams.Search));

        if (!string.IsNullOrEmpty(queryParams.Sort))
            query = ApplySorting(query, queryParams.Sort, queryParams.SortDirection);
        else
            query = query.OrderByDescending(c => c.CreatedAt);

        var totalCount = await query.CountAsync();

        var skip = (queryParams.Page - 1) * queryParams.PageSize;

        var customers = await query.Skip(skip)
                            .Take(queryParams.PageSize)
                            .ToListAsync();

        var customersDto = _mapper.Map<IEnumerable<CustomerResponseDto>>(customers);

        return PagedResult<CustomerResponseDto>.Create(
            customersDto,
            queryParams.Page,
            queryParams.PageSize,
            totalCount);
    }

    private IQueryable<Customer> ApplySorting(IQueryable<Customer> query, string sort, string sortDirection)
    {
        var isDescending = sortDirection.ToLower() == "desc";

        return sort.ToLower() switch
        {
            "name" => isDescending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),

            "email" => isDescending ? query.OrderByDescending(c => c.Email) : query.OrderBy(c => c.Email),

            "createdat" => isDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt),

            _ => query.OrderByDescending(c => c.CreatedAt)
        };
    }
}
