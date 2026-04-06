namespace Korp.InventoryService.Shared.DTOs.Product.GetProducts;

public class GetProductsResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Stock { get; set; }
    public int Reserved { get; set; }
    public int Available { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}