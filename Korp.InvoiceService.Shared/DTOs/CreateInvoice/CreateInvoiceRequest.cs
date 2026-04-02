namespace Korp.InvoiceService.Shared.DTOs.CreateInvoice;

public class CreateInvoiceRequest
{
    public List<CreateInvoiceitemRequest> InvoiceItems { get; set; } = new();
}
