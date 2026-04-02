using System.Text.Json.Serialization;

namespace Korp.InvoiceService.Shared.DTOs.GetInvoices;

public class GetInvoiceResponse
{
    public int Id { get; set; }
    public string Status { get => InvoiceStatus == "Closed" ? InvoiceStatus : "Open"; }
    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public string InvoiceStatus { get; set; } = null!;

    public IEnumerable<GetInvoiceItemResponse>? items { get; set; }
}
