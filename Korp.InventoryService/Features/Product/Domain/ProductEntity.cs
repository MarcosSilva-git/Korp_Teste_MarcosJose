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

    private ProductEntity() { }

    public ProductEntity(string name, int quantity)
    {
        ChangeName(name);
        AddStock(quantity);
    }

    public void ChangeName(string name)
    {
        if (name.Length == 0)
            throw new ArgumentException("Name cannot be empty.", nameof(name));

        Name = name;
    }

    public void AddStock(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Quantity must be greater then zero.", nameof(quantity));

        Stock += quantity;
    }

    public void RemoveStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater then zero.", nameof(quantity));

        if (Available < quantity)
            throw new InvalidOperationException($"Insufficient stock to remove. Available: {Available}, Requested: {quantity}.");

        Stock -= quantity;
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
