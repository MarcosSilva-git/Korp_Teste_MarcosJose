using Microsoft.AspNetCore.Mvc;

namespace Korp.InvoiceService.Features.GetInvoices;

[ApiController]
public class GetInvoicesController(GetInvoicesHandler handler) : ControllerBase
{
    private readonly GetInvoicesHandler _handler = handler;

    [HttpGet("api/invoices")]
    public async Task<IActionResult> GetAll()
    {
        var invoices = await _handler.HandleAsync();
        return Ok(new { invoices });
    }
}
