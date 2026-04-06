using Korp.InventoryService.Features.Product.Domain.Enums;
using Korp.InventoryService.Infraestructure;

namespace Korp.InventoryService.Features.Product.Domain;

public class ReservedProductEntity : EntityBase
{
    public int Id { get; private set; }
    public int ProductId { get; private set; }
    public Guid SagaId { get; private set; }
    public string OriginId { get; private set; }
    public string OriginType { get; private set; }
    public int Quantity { get; private set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public StockMovementStatusEnum StockMovementStatus { get; private set; } = StockMovementStatusEnum.Reserved;

    public ProductEntity? Product { get; private set; }

    public ReservedProductEntity(int productId, Guid sagaId, int quantity, string originId, string originType)
    {
        ProductId = productId;
        SagaId = sagaId;
        Quantity = quantity;
        OriginId = originId;
        OriginType = originType;
    }

    public void UpdateQuantity(int quantity)
    {
        Quantity = quantity;

        if (Quantity <= 0)
            throw new InvalidOperationException("Quantity must be greater than zero.");
    }

    public void MarkAsCommitted()
    {
        EnsureIsPending();
        StockMovementStatus = StockMovementStatusEnum.Committed;
    }

    public void MarkAsRolledBack()
    {
        EnsureIsPending();
        StockMovementStatus = StockMovementStatusEnum.RolledBack;
    }

    private void EnsureIsPending()
    {
        if (StockMovementStatus != StockMovementStatusEnum.Reserved)
            throw new InvalidOperationException($"Reservation is already finalized as {StockMovementStatus}.");
    }
}
