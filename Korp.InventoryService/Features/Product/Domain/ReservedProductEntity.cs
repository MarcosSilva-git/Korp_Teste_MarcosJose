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
    public DateTime? RolledbackAt { get; set; }
    public DateTime? DeletedAt { get; set; }

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
}
