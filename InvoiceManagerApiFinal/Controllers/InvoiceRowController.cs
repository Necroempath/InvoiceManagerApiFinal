using InvoiceManagerApi.Common;
using InvoiceManagerApi.DTOs.InvoiceRowDTOs;
using InvoiceManagerApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceManagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoiceRowController : ControllerBase
{
    private readonly IInvoiceRowService _service;

    public InvoiceRowController(IInvoiceRowService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieves all invoice rows.
    /// </summary>
    /// <remarks>
    /// Returns a list of all invoice rows.
    /// </remarks>
    /// <returns>A list of invoice rows wrapped in ApiResponse.</returns>
    /// <response code="200">Invoice rows were successfully retrieved.</response>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<InvoiceRowResponseDto>>>> GetAll()
    {
        var invoiceRows = await _service.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<InvoiceRowResponseDto>>.SuccessResponse(invoiceRows));
    }

    /// <summary>
    /// Retrieves an invoice row by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice row.</param>
    /// <returns>The requested invoice row wrapped in ApiResponse.</returns>
    /// <response code="200">Invoice row was successfully retrieved.</response>
    /// <response code="404">Invoice row with the specified id was not found.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<InvoiceRowResponseDto>>> GetById(int id)
    {
        var invoiceRow = await _service.GetByIdAsync(id);

        if (invoiceRow is null)
            return NotFound(ApiResponse<InvoiceRowResponseDto>.ErrorResponse($"InvoiceRow by given id {id} not found"));

        return Ok(ApiResponse<InvoiceRowResponseDto>.SuccessResponse(invoiceRow));
    }

    /// <summary>
    /// Retrieves invoice rows by related invoice's identifier.
    /// </summary>
    /// <param name="invoiceId">The unique identifier of the related invoice.</param>
    /// <returns>A list of invoice rows wrapped in ApiResponse.</returns>
    /// <response code="200">Invoice rows were successfully retrieved.</response>
    [HttpGet("/invoiceId/{invoiceId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<InvoiceRowResponseDto>>>> GetByInvoiceId(int invoiceId)
    {
        var invoiceRows = await _service.GetByInvoiceIdAsync(invoiceId);
        return Ok(ApiResponse<IEnumerable<InvoiceRowResponseDto>>.SuccessResponse(invoiceRows));
    }

    /// <summary>
    /// Creates a new invoice row.
    /// </summary>
    /// <param name="request">Invoice row creation data.</param>
    /// <returns>The newly created invoice row wrapped in ApiResponse.</returns>
    /// <response code="201">Invoice row was successfully created.</response>
    /// <response code="400">The request body is invalid or the related invoice was not found.</response>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<InvoiceRowResponseDto>>> Create([FromBody] InvoiceRowCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<InvoiceRowResponseDto>.ErrorResponse("Invalid request data"));

        var invoiceRow = await _service.CreateAsync(request);

        if (invoiceRow is null)
            return BadRequest(ApiResponse<InvoiceRowResponseDto>.ErrorResponse("Invoice by given ID not found"));

        return CreatedAtAction(
            nameof(GetById),
            new { id = invoiceRow.Id },
            ApiResponse<InvoiceRowResponseDto>.SuccessResponse(invoiceRow, "Invoice row created successfully"));
    }

    /// <summary>
    /// Permanently deletes an invoice row.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice row.</param>
    /// <response code="200">Invoice row was successfully permanently deleted.</response>
    /// <response code="404">Invoice row with the specified id was not found.</response>
    [HttpDelete("hard/{id}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteHard(int id)
    {
        bool isDeleted = await _service.DeleteHardAsync(id);

        if (!isDeleted)
            return NotFound(ApiResponse<object>.ErrorResponse($"InvoiceRow by given id {id} not found"));

        return Ok(ApiResponse<object>.SuccessResponse(null, "Invoice row permanently deleted successfully"));
    }

    /// <summary>
    /// Updates an existing invoice row.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice row.</param>
    /// <param name="request">Updated invoice row data.</param>
    /// <returns>The updated invoice row wrapped in ApiResponse.</returns>
    /// <response code="200">Invoice row was successfully updated.</response>
    /// <response code="404">Invoice row with the specified id was not found.</response>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<InvoiceRowResponseDto>>> Update(int id, [FromBody] InvoiceRowUpdateRequest request)
    {
        var invoiceRow = await _service.UpdateAsync(id, request);

        if (invoiceRow is null)
            return NotFound(ApiResponse<InvoiceRowResponseDto>.ErrorResponse($"InvoiceRow by given id {id} not found"));

        return Ok(ApiResponse<InvoiceRowResponseDto>.SuccessResponse(invoiceRow, "Invoice row updated successfully"));
    }
}
