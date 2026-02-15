using AutoMapper;
using InvoiceManagerApi.Data;
using InvoiceManagerApi.DTOs.InvoiceRowDTOs;
using InvoiceManagerApi.Models;
using InvoiceManagerApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManagerApi.Services.Implementations;

public class InvoiceRowService : IInvoiceRowService
{
    private readonly InvoiceManagerDbContext _context;
    private readonly IMapper _mapper;

    public InvoiceRowService(InvoiceManagerDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<InvoiceRowResponseDto?> CreateAsync(InvoiceRowCreateRequest request)
    {
        var invoice = await _context.Invoices.FirstOrDefaultAsync(c => c.Id == request.InvoiceId);

        if (invoice is null) return null;

        var invoiceRow = _mapper.Map<InvoiceRow>(request);

        await _context.InvoiceRows.AddAsync(invoiceRow);

        invoice.TotalSum += request.Quantity * request.Rate;

        await _context.SaveChangesAsync();

        return _mapper.Map<InvoiceRowResponseDto>(invoiceRow);
    }

    public async Task<bool> DeleteHardAsync(int id)
    {
        InvoiceRow? invoiceRow = await _context.InvoiceRows
             .FirstOrDefaultAsync(ir => ir.Id == id);

        if (invoiceRow is null) return false;

        _context.InvoiceRows.Remove(invoiceRow);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<InvoiceRowResponseDto>> GetAllAsync()
    {
        var invoiceRows = await _context.InvoiceRows
                .Include(i => i.Invoice)
                .ToListAsync();

        return _mapper.Map<IEnumerable<InvoiceRowResponseDto>>(invoiceRows);
    }

    public async Task<InvoiceRowResponseDto?> GetByIdAsync(int id)
    {
        var invoiceRow = await _context.InvoiceRows
                .FirstOrDefaultAsync(ir => ir.Id == id);

        if (invoiceRow is null) return null;

        return _mapper.Map<InvoiceRowResponseDto>(invoiceRow);
    }

    public async Task<IEnumerable<InvoiceRowResponseDto>> GetByInvoiceIdAsync(int invoiceId)
    {
        var invoiceRows = await _context.InvoiceRows
            .Where(ir => ir.InvoiceId == invoiceId)
            .Include(ir => ir.Invoice)
            .ToListAsync();

        return _mapper.Map<IEnumerable<InvoiceRowResponseDto>>(invoiceRows);
    }

    public async Task<InvoiceRowResponseDto?> UpdateAsync(int id, InvoiceRowUpdateRequest request)
    {
        var invoiceRow = await _context.InvoiceRows
        .FirstOrDefaultAsync(ir => ir.Id == id);

        if (invoiceRow is null) return null;

        var invoice = await _context.Invoices
            .FirstOrDefaultAsync(i => 
            i.Rows.Any(ir => ir.Id == id));

        invoice!.TotalSum += (request.Rate * request.Quantity) - invoiceRow.Sum;

        _mapper.Map(request, invoiceRow);

        await _context.SaveChangesAsync();

        return _mapper.Map<InvoiceRowResponseDto>(invoiceRow);
    }
}
