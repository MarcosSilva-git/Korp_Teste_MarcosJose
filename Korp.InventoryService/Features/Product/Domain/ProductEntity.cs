namespace Korp.InventoryService.Features.Product.Domain;

public class ProductEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Balance { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }
}
