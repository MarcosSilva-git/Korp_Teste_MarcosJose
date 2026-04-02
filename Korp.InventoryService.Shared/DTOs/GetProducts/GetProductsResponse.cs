namespace Korp.InventoryService.Shared.DTOs.GetProducts;

public class GetProductsResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Balance { get; set; }
    public DateTime CreatedAt { get; set; }
}
