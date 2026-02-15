using AutoMapper;
using InvoiceManagerApi.Common;
using InvoiceManagerApi.Data;
using InvoiceManagerApi.DTOs.InvoiceDTOs;
using InvoiceManagerApi.Enums;
using InvoiceManagerApi.Models;
using InvoiceManagerApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace InvoiceManagerApi.Services.Implementations;

public class InvoiceService : IInvoiceService
{
    private readonly InvoiceManagerDbContext _context;
    private readonly IMapper _mapper;

    public InvoiceService(InvoiceManagerDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<InvoiceResponseDto?> CreateAsync(InvoiceCreateRequest request)
    {
        var isCustomerExists = await _context.Customers.AnyAsync(c => c.DeletedAt == null && c.Id == request.CustomerId);
    
        if (!isCustomerExists) return null;

        var invoice = _mapper.Map<Invoice>(request);

        await _context.Invoices.AddAsync(invoice);

        await _context.SaveChangesAsync();

        return _mapper.Map<InvoiceResponseDto>(invoice);
    }

    public async Task<bool> DeleteHardAsync(int id)
    {
        Invoice? invoice = await _context.Invoices
            .FirstOrDefaultAsync(i => i.DeletedAt == null && i.Id == id && i.Status == InvoiceStatus.Created);

        if (invoice is null) return false;

        _context.Invoices.Remove(invoice);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteSoftAsync(int id)
    {
        Invoice? invoice = await _context.Invoices
            .FirstOrDefaultAsync(i => i.DeletedAt == null && i.Id == id);

        if (invoice is null) return false;

        invoice.DeletedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<PagedResult<InvoiceResponseDto>> GetPagedResultAsync(InvoiceQueryParams queryParams)
    {
        queryParams.Validate();

        var query = _context.Invoices
                    .Where(i => i.DeletedAt == null)
                    .Include(i => i.Customer)
                    .Include(i => i.Rows)
                    .AsQueryable();
        
        if (queryParams.CustomerId.HasValue)
            query = query.Where(i => i.CustomerId == queryParams.CustomerId.Value);
        
        if (!string.IsNullOrWhiteSpace(queryParams.Search))
             query = query.Where(i => i.Comment != null && i.Comment.ToLower().Contains(queryParams.Search));

        if (!string.IsNullOrWhiteSpace(queryParams.Status))
        {
            if (Enum.TryParse<InvoiceStatus>(queryParams.Status, out var status))
            {
                query = query.Where(i => i.Status == status);
            }
        }

        if (queryParams.MinSum.HasValue)
            query = query.Where(i => i.TotalSum >= queryParams.MinSum.Value);

        if (queryParams.MaxSum.HasValue)
            query = query.Where(i => i.TotalSum <= queryParams.MaxSum.Value);

        if (!string.IsNullOrEmpty(queryParams.Sort))
            query = ApplySorting(query, queryParams.Sort, queryParams.SortDirection);
        else
            query = query.OrderByDescending(c => c.CreatedAt);

        var totalCount = await query.CountAsync();
        
        var skip = (queryParams.Page - 1) * queryParams.PageSize;
        
        var invoices = await query.Skip(skip)
            .Take(queryParams.PageSize)
            .ToListAsync();
        
        var invoiceDtos = _mapper.Map<IEnumerable<InvoiceResponseDto>>(invoices);

        return PagedResult<InvoiceResponseDto>.Create(invoiceDtos, queryParams.Page, queryParams.PageSize, totalCount);
    }

    public async Task<IEnumerable<InvoiceResponseDto>> GetAllAsync()
    {
        var invoices = await _context.Invoices
                        .Where(i => i.DeletedAt == null)
                        .Include(i => i.Customer)
                        .Include(i => i.Rows)
                        .ToListAsync();

        return _mapper.Map<IEnumerable<InvoiceResponseDto>>(invoices);
    }

    public async Task<IEnumerable<InvoiceResponseDto>> GetByCustomerIdAsync(int customerId)
    {
        var invoices = await _context.Invoices
            .Where(i => i.CustomerId == customerId && i.DeletedAt == null)
            .Include(i => i.Customer)
            .Include(i => i.Rows)
            .ToListAsync();

        return _mapper.Map<IEnumerable<InvoiceResponseDto>> (invoices);
    }

    public async Task<InvoiceResponseDto?> GetByIdAsync(int id)
    {
        var invoice = await _context.Invoices
                        .Include(i => i.Customer)
                        .Include(i => i.Rows)
                        .FirstOrDefaultAsync(i => i.DeletedAt == null && i.Id == id);

        if (invoice is null) return null;

        return _mapper.Map<InvoiceResponseDto>(invoice);
    }

    public async Task<InvoiceResponseDto?> StatusChangeAsync(int id, InvoiceStatusChangeRequest request)
    {
        var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Rows)
                .FirstOrDefaultAsync(i => i.DeletedAt == null && i.Id == id);

        if (invoice is null) return null;
        
        invoice.Status = request.Status;

        await _context.SaveChangesAsync();

        return _mapper.Map<InvoiceResponseDto>(invoice);
    }

    public async Task<InvoiceResponseDto?> UpdateAsync(int id, InvoiceUpdateRequest request)
    {
        var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Rows)
                .FirstOrDefaultAsync(i => i.DeletedAt == null && i.Id == id && i.Status == InvoiceStatus.Created);

        if (invoice is null) return null;

        _mapper.Map(request, invoice);

        await _context.SaveChangesAsync();

        return _mapper.Map<InvoiceResponseDto>(invoice);
    }

    private IQueryable<Invoice> ApplySorting(IQueryable<Invoice> query, string sort, string sortDirection)
    {
        var isDescending = sortDirection.ToLower() == "desc";

        return sort.ToLower() switch
        {
            "startdate" => isDescending ? query.OrderByDescending(i => i.StartDate) : query.OrderBy(s => s.StartDate),

            "enddate" => isDescending ? query.OrderByDescending(i => i.EndDate) : query.OrderBy(s => s.EndDate),

            "totalsum" => isDescending ? query.OrderByDescending(i => i.TotalSum) : query.OrderBy(i => i.TotalSum),

            "createdat" => isDescending ? query.OrderByDescending(i => i.CreatedAt) : query.OrderBy(s => s.CreatedAt),

            _ => query.OrderByDescending(i => i.CreatedAt),
        };
    }
}
