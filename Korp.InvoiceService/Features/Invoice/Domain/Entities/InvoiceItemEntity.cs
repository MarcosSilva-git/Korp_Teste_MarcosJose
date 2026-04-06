using Korp.InvoiceService.Features.Invoice.Domain.Exceptions;

namespace Korp.InvoiceService.Features.Invoice.Domain.Entities;

public class InvoiceItemEntity
{
    public int Id { get; private set; }
    public int InvoiceEntityId { get; private set; }
    public int ItemSequence { get; private set; }
    public int ProductId { get; private set; }
    public string ProductName { get; private set; } = null!;
    public int Quantity { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? DeletedAt { get; set; }

    protected InvoiceItemEntity() { }

    public InvoiceItemEntity(
        int productId,
        string productName,
        int quantity,
        int itemSequence)
    {
        UpdateQuantity(quantity);

        ProductId = productId;
        ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
        ItemSequence = itemSequence;
    }

    public void UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Invoice quantity must be greater than zero");

        Quantity = quantity;
    }
}
