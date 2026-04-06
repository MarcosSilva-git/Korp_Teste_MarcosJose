using System.Text.Json.Serialization;

namespace Korp.InvoiceService.Shared.DTOs.Invoice.GetInvoices;

public class GetInvoiceResponse
{
    public int Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Status { get; set; } = null!;

    public IEnumerable<GetInvoiceItemResponse>? items { get; set; }
}
