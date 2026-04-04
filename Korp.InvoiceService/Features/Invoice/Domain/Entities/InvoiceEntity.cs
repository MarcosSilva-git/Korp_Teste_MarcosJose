using Korp.InvoiceService.Features.Invoice.Domain.Enums;
using Korp.InvoiceService.Features.Invoice.Domain.Exceptions;

namespace Korp.InvoiceService.Features.Invoice.Domain.Entities;

public class InvoiceEntity
{
    public int Id { get; private set; }
    public InvoiceStatusEnum InvoiceStatus { get; private set; } = InvoiceStatusEnum.Processing;
    public Guid SagaId { get; private set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }

    public ICollection<InvoiceItemEntity> InvoiceItems { get; private set; } = new List<InvoiceItemEntity>();


    protected InvoiceEntity() { }

    public InvoiceEntity(ICollection<InvoiceItemEntity> items)
    {
        InvoiceItems = items;
    }

    public void MarkAsOpen()
    {
        if (InvoiceStatus != InvoiceStatusEnum.Processing)
            throw new InvalidInvoiceStateException("Cannot mark invoice as OPEN", InvoiceStatus);

        InvoiceStatus = InvoiceStatusEnum.Open;
    }

    public void MarkAsClosed()
    {
        if (InvoiceStatus != InvoiceStatusEnum.Open)
            throw new InvalidInvoiceStateException("Only OPEN invoices can be CLOSED", InvoiceStatus);

        InvoiceStatus = InvoiceStatusEnum.Closed;
    }

    public void MarkAsCancelled()
    {
        if (InvoiceStatus != InvoiceStatusEnum.Processing)
            throw new InvalidInvoiceStateException("Only PROCESSING invoices can be CANCELLED", InvoiceStatus);

        InvoiceStatus = InvoiceStatusEnum.Cancelled;
    }
}

