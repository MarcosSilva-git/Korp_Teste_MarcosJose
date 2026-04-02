using Korp.InvoiceService.Shared.DTOs.CreateInvoice;
using Microsoft.AspNetCore.Mvc;

namespace Korp.InvoiceService.Features.CreateInvoice;

[ApiController]
public class CreateInvoiceController(CreateInvoiceHandler createInvoiceHandler) : ControllerBase
{
    private readonly CreateInvoiceHandler _createInvoiceHandler = createInvoiceHandler;

    [HttpPost("api/invoices")]
    public async Task<IActionResult> Add(CreateInvoiceRequest request)
    {
        var result = await _createInvoiceHandler.HandleAsync(request);

        return result.Match<IActionResult>(
            id => CreatedAtAction(nameof(Add), new { id }, null),
            BadRequest);
    }
}
