using Korp.InvoiceService.Shared.DTOs.Invoice.CreateInvoice;
using Korp.InvoiceService.Shared.DTOs.Invoice.CloseInvoice;
using Korp.InvoiceService.Shared.DTOs.Invoice.GetInvoices;
using Korp.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Korp.Shared;

namespace Korp.InvoiceService.Features.Invoice;

[ApiController]
[Route("api/[controller]")]
public class InvoicesController(IDispatcher _dispatcher) : SharedControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetInvoicesQuery query, CancellationToken ct)
    {
        var result = await _dispatcher.SendAsync(query, ct);

        if (result.IsFailure)
            return CreateValidationProblemFromResult(result);

        return Ok(new { Data = result.Value });
    }

    [HttpPost]
    public async Task<IActionResult> Add(CreateInvoiceRequest request)
        => Ok(await _dispatcher.SendAsync(request));

    [HttpPost("{id}/close")]
    public async Task<IActionResult> Close(int id, CancellationToken ct)
    {
        var result = await _dispatcher.SendAsync(new CloseInvoiceRequest(id), ct);
        if (result.IsFailure)
            return CreateValidationProblemFromResult(result);

        return Ok(result.Value);
    }   
}