namespace Korp.InvoiceService.Entities;

public class InvoiceEntity
{
    public int Code { get; set; }
    public InvoiceStatusEnum InvoiceStatus { get; set; }
    public ICollection<InvoiceItem>? InvoiceItems { get; set; }

}
