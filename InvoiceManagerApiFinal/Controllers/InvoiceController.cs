using InvoiceManagerApi.Common;
using InvoiceManagerApi.DTOs.InvoiceDTOs;
using InvoiceManagerApi.Models;
using InvoiceManagerApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceManagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _service;

    public InvoiceController(IInvoiceService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieves all invoices.
    /// </summary>
    /// <remarks>
    /// Returns a list of all invoices that are not soft-deleted.
    /// </remarks>
    /// <returns>A list of invoices wrapped in ApiResponse.</returns>
    /// <response code="200">Invoices were successfully retrieved.</response>
    [HttpGet("all")]
    public async Task<ActionResult<ApiResponse<IEnumerable<InvoiceResponseDto>>>> GetAll()
    {
        var invoices = await _service.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<InvoiceResponseDto>>.SuccessResponse(invoices));
    }

    /// <summary>
    /// Retrieves all invoices in the page range.
    /// </summary>
    /// <remarks>
    /// Returns a list of all invoices in the page range that are not soft-deleted.
    /// </remarks>
    /// <returns>
    /// A list of invoices wrapped in ApiResponse.
    /// </returns>
    /// <response code="200">Invoices were successfully retrieved.</response>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<IEnumerable<InvoiceResponseDto>>>>> GetPaged([FromQuery]InvoiceQueryParams queryParams)
    {
        var result = await _service.GetPagedResultAsync(queryParams);
        
        return Ok(ApiResponse<PagedResult<InvoiceResponseDto>>.SuccessResponse(result));
    }

    /// <summary>
    /// Retrieves an invoice by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice.</param>
    /// <returns>The requested invoice wrapped in ApiResponse.</returns>
    /// <response code="200">Invoice was successfully retrieved.</response>
    /// <response code="404">Invoice with the specified id was not found.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<InvoiceResponseDto>>> GetById(int id)
    {
        var invoice = await _service.GetByIdAsync(id);

        if (invoice is null)
            return NotFound(ApiResponse<InvoiceResponseDto>.ErrorResponse($"Invoice by given id {id} not found"));

        return Ok(ApiResponse<InvoiceResponseDto>.SuccessResponse(invoice));
    }

    /// <summary>
    /// Retrieves invoices by related customer's identifier.
    /// </summary>
    /// <param name="customerId">The unique identifier of the related customer.</param>
    /// <returns>A list of invoices wrapped in ApiResponse.</returns>
    /// <response code="200">Invoices were successfully retrieved.</response>
    [HttpGet("/customerId/{customerId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<InvoiceResponseDto>>>> GetByCustomerId(int customerId)
    {
        var invoices = await _service.GetByCustomerIdAsync(customerId);
        return Ok(ApiResponse<IEnumerable<InvoiceResponseDto>>.SuccessResponse(invoices));
    }

    /// <summary>
    /// Creates a new invoice.
    /// </summary>
    /// <param name="request">Invoice creation data.</param>
    /// <returns>The newly created invoice wrapped in ApiResponse.</returns>
    /// <response code="201">Invoice was successfully created.</response>
    /// <response code="400">The request body is invalid or the related customer was not found.</response>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<InvoiceResponseDto>>> Create([FromBody] InvoiceCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<InvoiceResponseDto>.ErrorResponse("Invalid request data"));

        var invoice = await _service.CreateAsync(request);

        if (invoice is null)
            return BadRequest(ApiResponse<InvoiceResponseDto>.ErrorResponse("Customer by given ID not found"));

        return CreatedAtAction(
            nameof(GetById),
            new { id = invoice.Id },
            ApiResponse<InvoiceResponseDto>.SuccessResponse(invoice, "Invoice created successfully"));
    }

    /// <summary>
    /// Soft deletes an invoice.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice.</param>
    /// <response code="200">Invoice was successfully soft deleted.</response>
    /// <response code="404">Invoice with the specified id was not found.</response>
    [HttpDelete("soft/{id}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteSoft(int id)
    {
        bool isDeleted = await _service.DeleteSoftAsync(id);

        if (!isDeleted)
            return NotFound(ApiResponse<object>.ErrorResponse($"Invoice by given id {id} not found"));

        return Ok(ApiResponse<object>.SuccessResponse(null, "Invoice soft deleted successfully"));
    }

    /// <summary>
    /// Permanently deletes an invoice.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice.</param>
    /// <response code="200">Invoice was successfully permanently deleted.</response>
    /// <response code="404">Invoice with the specified id was not found.</response>
    [HttpDelete("hard/{id}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteHard(int id)
    {
        bool isDeleted = await _service.DeleteHardAsync(id);

        if (!isDeleted)
            return NotFound(ApiResponse<object>.ErrorResponse($"Invoice by given id {id} not found"));

        return Ok(ApiResponse<object>.SuccessResponse(null, "Invoice permanently deleted successfully"));
    }

    /// <summary>
    /// Updates an existing invoice.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice.</param>
    /// <param name="request">Updated invoice data.</param>
    /// <returns>The updated invoice wrapped in ApiResponse.</returns>
    /// <response code="200">Invoice was successfully updated.</response>
    /// <response code="404">Invoice with the specified id was not found.</response>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<InvoiceResponseDto>>> Update(int id, [FromBody] InvoiceUpdateRequest request)
    {
        var invoice = await _service.UpdateAsync(id, request);

        if (invoice is null)
            return NotFound(ApiResponse<InvoiceResponseDto>.ErrorResponse($"Invoice by given id {id} not found"));

        return Ok(ApiResponse<InvoiceResponseDto>.SuccessResponse(invoice, "Invoice updated successfully"));
    }

    /// <summary>
    /// Changes the status of an existing invoice.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice.</param>
    /// <param name="request">Invoice status change data.</param>
    /// <returns>The invoice with the updated status wrapped in ApiResponse.</returns>
    /// <response code="200">Invoice status was successfully updated.</response>
    /// <response code="400">Either the invoice id or the provided status is invalid.</response>
    [HttpPatch("{id}")]
    public async Task<ActionResult<ApiResponse<InvoiceResponseDto>>> StatusChange(int id, [FromBody] InvoiceStatusChangeRequest request)
    {
        var invoice = await _service.StatusChangeAsync(id, request);

        if (invoice is null)
            return BadRequest(ApiResponse<InvoiceResponseDto>.ErrorResponse("Either ID or Status is incorrect"));

        return Ok(ApiResponse<InvoiceResponseDto>.SuccessResponse(invoice, "Invoice status updated successfully"));
    }
}
