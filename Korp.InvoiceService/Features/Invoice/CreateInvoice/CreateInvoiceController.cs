using Korp.InvoiceService.Shared.DTOs.CreateInvoice;
using Microsoft.AspNetCore.Mvc;

namespace Korp.InvoiceService.Features.Invoice.CreateInvoice;

[ApiController]
public class CreateInvoiceController(CreateInvoiceHandler createInvoiceHandler) : ControllerBase
{
    private readonly CreateInvoiceHandler _createInvoiceHandler = createInvoiceHandler;

    [HttpPost("api/invoices")]
    public async Task<IActionResult> Add(CreateInvoiceRequest request)
    {
        var invoice = await _createInvoiceHandler.HandleAsync(request);
        return Ok(invoice);
    }
}
