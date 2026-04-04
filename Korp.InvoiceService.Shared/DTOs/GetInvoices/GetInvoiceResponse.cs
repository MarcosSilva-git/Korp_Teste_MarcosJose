using System.Text.Json.Serialization;

namespace Korp.InvoiceService.Shared.DTOs.GetInvoices;

public class GetInvoiceResponse
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = null!;

    public IEnumerable<GetInvoiceItemResponse>? items { get; set; }
}
