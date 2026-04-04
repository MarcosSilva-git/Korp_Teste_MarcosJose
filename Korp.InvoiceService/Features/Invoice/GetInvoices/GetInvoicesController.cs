using Microsoft.AspNetCore.Mvc;

namespace Korp.InvoiceService.Features.Invoice.GetInvoices;

[ApiController]
public class GetInvoicesController(GetInvoicesHandler handler) : ControllerBase
{
    private readonly GetInvoicesHandler _handler = handler;

    [HttpGet("api/invoices")]
    public async Task<IActionResult> Get([FromQuery] string? ids)
    {
        var result = await _handler.HandleAsync(ids);

        if (result.IsFailure)
        {
            var memberName = result.Error!.MemberNames.FirstOrDefault() ?? string.Empty;
            ModelState.AddModelError(memberName, result.Error.ErrorMessage ?? string.Empty);

            return ValidationProblem(ModelState);
        }

        return Ok(new { Invoices = result.Value });
    }
}
