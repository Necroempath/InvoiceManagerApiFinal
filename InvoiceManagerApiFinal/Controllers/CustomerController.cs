using InvoiceManagerApi.Common;
using InvoiceManagerApi.DTOs.CustomerDTOs;
using InvoiceManagerApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceManagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _service;

    public CustomerController(ICustomerService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieves all customers.
    /// </summary>
    /// <remarks>
    /// Returns a list of all customers that are not soft-deleted.
    /// </remarks>
    /// <returns>
    /// A list of customers wrapped in ApiResponse.
    /// </returns>
    /// <response code="200">Customers were successfully retrieved.</response>
    [HttpGet("all")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CustomerResponseDto>>>> GetAll()
    {
        var customers = await _service.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<CustomerResponseDto>>.SuccessResponse(customers));
    }

    /// <summary>
    /// Retrieves all customers in the page range.
    /// </summary>
    /// <remarks>
    /// Returns a list of all customers in the page range that are not soft-deleted.
    /// </remarks>
    /// <returns>
    /// A list of customers wrapped in ApiResponse.
    /// </returns>
    /// <response code="200">Customers were successfully retrieved.</response>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<IEnumerable<CustomerResponseDto>>>>> GetPaged([FromQuery]CustomerQueryParams queryParams)
    {
        var result = await _service.GetPagedResultAsync(queryParams);

        return Ok(ApiResponse<PagedResult<CustomerResponseDto>>.SuccessResponse(result));
    }

    /// <summary>
    /// Retrieves a customer by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the customer.</param>
    /// <returns>
    /// The requested customer wrapped in ApiResponse.
    /// </returns>
    /// <response code="200">Customer was successfully retrieved.</response>
    /// <response code="404">Customer with the specified id was not found.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CustomerResponseDto>>> GetById(int id)
    {
        var customer = await _service.GetByIdAsync(id);

        if (customer is null)
            return NotFound(ApiResponse<CustomerResponseDto>.ErrorResponse($"Customer by given id {id} not found"));

        return Ok(ApiResponse<CustomerResponseDto>.SuccessResponse(customer));
    }

    /// <summary>
    /// Creates a new customer.
    /// </summary>
    /// <param name="request">Customer creation data.</param>
    /// <returns>
    /// The newly created customer wrapped in ApiResponse.
    /// </returns>
    /// <response code="201">Customer was successfully created.</response>
    /// <response code="400">The request body is invalid.</response>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CustomerResponseDto>>> Create([FromBody] CustomerCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<CustomerResponseDto>.ErrorResponse("Invalid request data"));

        var customer = await _service.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = customer.Id },
            ApiResponse<CustomerResponseDto>.SuccessResponse(customer, "Customer created successfully"));
    }

    /// <summary>
    /// Soft deletes a customer.
    /// </summary>
    /// <remarks>
    /// Marks the customer as deleted without removing it from the database.
    /// </remarks>
    /// <param name="id">The unique identifier of the customer.</param>
    /// <response code="200">Customer was successfully soft deleted.</response>
    /// <response code="404">Customer with the specified id was not found.</response>
    [HttpDelete("soft/{id}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteSoft(int id)
    {
        bool isDeleted = await _service.DeleteSoftAsync(id);

        if (!isDeleted)
            return NotFound(ApiResponse<object>.ErrorResponse($"Customer by given id {id} not found"));

        return Ok(ApiResponse<object>.SuccessResponse(null, "Customer soft deleted successfully"));
    }

    /// <summary>
    /// Permanently deletes a customer.
    /// </summary>
    /// <remarks>
    /// Completely removes the customer from the database.
    /// This operation is irreversible.
    /// </remarks>
    /// <param name="id">The unique identifier of the customer.</param>
    /// <response code="200">Customer was successfully permanently deleted.</response>
    /// <response code="404">Customer with the specified id was not found.</response>
    [HttpDelete("hard/{id}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteHard(int id)
    {
        bool isDeleted = await _service.DeleteHardAsync(id);

        if (!isDeleted)
            return NotFound(ApiResponse<object>.ErrorResponse($"Customer by given id {id} not found"));

        return Ok(ApiResponse<object>.SuccessResponse(null, "Customer permanently deleted successfully"));
    }

    /// <summary>
    /// Updates an existing customer.
    /// </summary>
    /// <param name="id">The unique identifier of the customer.</param>
    /// <param name="request">Updated customer data.</param>
    /// <returns>
    /// The updated customer wrapped in ApiResponse.
    /// </returns>
    /// <response code="200">Customer was successfully updated.</response>
    /// <response code="404">Customer with the specified id was not found.</response>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CustomerResponseDto>>> Update(int id, [FromBody] CustomerUpdateRequest request)
    {
        var customer = await _service.UpdateAsync(id, request);

        if (customer is null)
            return NotFound(ApiResponse<CustomerResponseDto>.ErrorResponse($"Customer by given id {id} not found"));

        return Ok(ApiResponse<CustomerResponseDto>.SuccessResponse(customer, "Customer updated successfully"));
    }
}
