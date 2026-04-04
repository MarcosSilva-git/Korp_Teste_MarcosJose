using System.ComponentModel.DataAnnotations;

namespace Korp.InvoiceService.Shared.DTOs.CreateInvoice;

public class CreateInvoiceRequest
{
    [MinLength(1, ErrorMessage = "{0} must contain at least one item.")]
    public List<CreateInvoiceitemRequest> InvoiceItems { get; set; } = new();
}
