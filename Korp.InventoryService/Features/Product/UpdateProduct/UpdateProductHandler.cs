using Korp.InventoryService.Infraestructure;
using Korp.InventoryService.Shared.DTOs;
using Korp.InventoryService.Shared.DTOs.GetProducts;
using Korp.Shared.Abstractions;

namespace Korp.InventoryService.Features.Product.UpdateProduct;

public class UpdateProductHandler(InventoryDbContext inventoryDbContext)
{
    private readonly InventoryDbContext _inventoryDbContext = inventoryDbContext;

    public async Task<Result<GetProductsResponse, KeyNotFoundException>> HandlerAsync(int productId, UpdateProductRequest request)
    {
        var product = await _inventoryDbContext.Products.FindAsync(productId);

        if (product is null)
            return new KeyNotFoundException($"Product with id {productId} was not found.");

        product.Name = request.Name;
        product.Balance = request.Balance;

        await _inventoryDbContext.SaveChangesAsync();

        return new GetProductsResponse
        {
            Id = product.Id,
            Name = product.Name,
            Balance = product.Balance,
            CreatedAt = product.CreatedAt
        };
    }
}
