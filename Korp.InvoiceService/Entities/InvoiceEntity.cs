namespace Korp.InvoiceService.Entities;

public class InvoiceEntity
{
    public int Id { get; set; }
    public InvoiceStatusEnum InvoiceStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DeletedAt { get; set; }


    public ICollection<InvoiceItemEntity>? InvoiceItems { get; set; }
}
