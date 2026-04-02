using Korp.InventoryService.Infraestructure;
using Korp.Shared.Abstractions;

namespace Korp.InventoryService.Features.Product.DeleteProduct;

public class DeleteProductHandler(InventoryDbContext inventoryDbContext)
{
    private readonly InventoryDbContext _inventoryDbContext = inventoryDbContext;

    public async Task<Result<int, Exception>> HandleAsync(int productId)
    {
        var product = await _inventoryDbContext.Products.FindAsync(productId);

        if (product is null)
            return new FormatException($"Product with id {productId} was not found.");

        product.DeletedAt = DateTime.UtcNow;
        await _inventoryDbContext.SaveChangesAsync();

        return productId;
    }
}
