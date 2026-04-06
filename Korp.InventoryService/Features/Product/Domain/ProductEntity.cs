using Korp.InventoryService.Infraestructure;

namespace Korp.InventoryService.Features.Product.Domain;

public class ProductEntity : EntityBase
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Stock { get; private set; }
    public int Reserved { get; private set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }

    public int Available => Stock - Reserved;

    public ICollection<ReservedProductEntity>? Reservations { get; private set; }

    private ProductEntity() { }

    public ProductEntity(string name, int quantity)
    {
        ChangeName(name);
        UpdateStock(quantity);
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));

        Name = name;
    }

    public void UpdateStock(int newStock)
    {
        if (newStock < 0)
            throw new ArgumentException("Stock cannot be negative.", nameof(newStock));

        if (newStock < Stock && Available < newStock)
            throw new InvalidOperationException($"New stock cannot be less than available quantity. Available: {Available}, New Stock: {newStock}.");

        Stock = newStock;
    }

    public void AddReservation(int quantity)
    {
        if (Available < quantity)
            throw new InvalidOperationException($"Insufficient stock to reserve. Available: {Available}, Requested: {quantity}.");

        Reserved += quantity;
    }

    public void RemoveReservation(int quantity)
    {
        if (Reserved < quantity)
            throw new InvalidOperationException($"Cannot remove more reservations than currently reserved. Reserved: {Reserved}, Requested: {quantity}.");

        Reserved -= quantity;
    }
}
