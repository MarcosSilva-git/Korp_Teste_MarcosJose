namespace Korp.InvoiceService.Entities;

public class InvoiceItemEntity
{
    public int Id { get; set; }
    public int ItemSequence { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DeletedAt { get; set; }
}
