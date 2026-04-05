using Korp.InvoiceService.Shared.DTOs.GetInvoices;
using Korp.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Korp.InvoiceService.Shared.DTOs.CreateInvoice;

public class CreateInvoiceRequest : IRequest<GetInvoiceResponse>
{
    [MinLength(1, ErrorMessage = "{0} must contain at least one item.")]
    public List<CreateInvoiceitemRequest> InvoiceItems { get; set; } = new();
}
