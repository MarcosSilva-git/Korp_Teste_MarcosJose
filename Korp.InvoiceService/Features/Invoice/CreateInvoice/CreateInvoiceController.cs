using Korp.InvoiceService.Shared.DTOs.CreateInvoice;
using Korp.Shared.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Korp.InvoiceService.Features.Invoice.CreateInvoice;

[ApiController]
public class CreateInvoiceController(CreateInvoiceHandler createInvoiceHandler) : ControllerBase
{
    private readonly CreateInvoiceHandler _createInvoiceHandler = createInvoiceHandler;

    [HttpPost("api/invoices")]
    public async Task<IActionResult> Add(CreateInvoiceRequest request)
    {
        var result = await _createInvoiceHandler.HandleAsync(request);

        if (result.IsSuccess)
            return CreatedAtAction(nameof(Add), new { id = result.Value }, null);

        return Problem(
            title: "Failed to create invoice",
            detail: result.Error,
            statusCode: StatusCodes.Status400BadRequest);
    }
}
