using Korp.InventoryService.Infraestructure;
using Korp.InventoryService.Shared.DTOs;
using Korp.InventoryService.Shared.DTOs.GetProducts;
using Korp.Shared.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Korp.InventoryService.Features.Product.UpdateProduct;

public class UpdateProductHandler(InventoryDbContext inventoryDbContext)
{
    private readonly InventoryDbContext _inventoryDbContext = inventoryDbContext;

    public async Task<Result<GetProductsResponse, ValidationResult>> HandlerAsync(int productId, UpdateProductRequest request)
    {
        var product = await _inventoryDbContext.Products.FindAsync(productId);

        if (product is null)
            return new ValidationResult($"Product with id {productId} was not found.", [$"ProductId_{productId}"]);

        product.ChangeName(request.Name);

        if (product.Stock == 0)
        {
            product.UpdateStock(request.Stock);
        }
        else if (request.Stock < product.Stock && request.Stock < product.Reserved)
        {
            return new ValidationResult(
                $"The requested quantity exceeds the current available balance for this product. Available: {product.Available}, Requested: {request.Stock}", 
                [$"ProductId_{productId}"]);
        }
        else
        {
            throw new InvalidOperationException(
                $"Invalid stock transition for ProductId: {productId}. " +
                $"Available: {product.Available}, " +
                $"Reserved: {product.Reserved}, " +
                $"Requested: {request.Stock}");
        }

        await _inventoryDbContext.SaveChangesAsync();

        return new GetProductsResponse
        {
            Id = product.Id,
            Name = product.Name,
            Stock = product.Stock,
            Reserved = product.Reserved,
            Available = product.Available,
            CreatedAt = product.CreatedAt
        };
    }
}
