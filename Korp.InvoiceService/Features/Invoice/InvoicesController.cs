using Korp.InvoiceService.Shared.DTOs.Invoice.CreateInvoice;
using Korp.InvoiceService.Shared.DTOs.Invoice.DeleteInvoice;
using Korp.InvoiceService.Shared.DTOs.Invoice.GetInvoices;
using Korp.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Korp.InvoiceService.Features.Invoice;

[ApiController]
[Route("api/[controller]")]
public class InvoicesController(IDispatcher _dispatcher) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetInvoicesQuery query, CancellationToken ct)
    {
        var result = await _dispatcher.SendAsync(query, ct);

        if (result.IsFailure)
        {
            var memberName = result.Error!.MemberNames.FirstOrDefault() ?? string.Empty;
            ModelState.AddModelError(memberName, result.Error.ErrorMessage ?? string.Empty);

            return ValidationProblem(ModelState);
        }

        return Ok(new { Data = result.Value });
    }

    [HttpPost]
    public async Task<IActionResult> Add(CreateInvoiceRequest request)
        => Ok(await _dispatcher.SendAsync(request));

    [HttpPatch("{id}/close")]
    public async Task<IActionResult> Close(int id, CancellationToken ct)
    {
        var result = await _dispatcher.SendAsync(new CloseInvoiceRequest(id), ct);
        if (result.IsFailure)
        {
            var memberName = result.Error!.MemberNames.FirstOrDefault() ?? string.Empty;
            ModelState.AddModelError(memberName, result.Error.ErrorMessage ?? string.Empty);
            return ValidationProblem(ModelState);
        }
        return Ok(result.Value);
    }   
}