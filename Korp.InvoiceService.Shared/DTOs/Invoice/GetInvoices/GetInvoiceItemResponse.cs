namespace Korp.InvoiceService.Shared.DTOs.Invoice.GetInvoices;

public class GetInvoiceItemResponse
{
    public int Id { get; set; } 
    public int InvoiceId { get; set; }
    public int ItemSequence { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }
    public DateTime CreatedDate { get; set; }
}