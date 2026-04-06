using Korp.InventoryService.Infraestructure;
using Korp.InventoryService.Shared.DTOs.Product.DeleteProduct;
using Korp.Shared.Abstractions;
using Korp.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Korp.InventoryService.Features.Product.DeleteProduct;

public class DeleteProductHandler(InventoryDbContext _inventoryDbContext)
    : IRequestHandlerAsync<DeleteProductRequest, Result<DeleteProductResponse, ValidationResult>>
{
    public async Task<Result<DeleteProductResponse, ValidationResult>> HandleAsync(DeleteProductRequest command, CancellationToken ct)
    {
        var product = await _inventoryDbContext.Products.FindAsync(command.ProductId);

        if (product is null)
            return new ValidationResult($"Product with id {command.ProductId} was not found.", [ $"ProductId_{command.ProductId}" ]);

        product.DeletedAt = DateTimeOffset.UtcNow;
        await _inventoryDbContext.SaveChangesAsync();

        return new DeleteProductResponse(command.ProductId);
    }
}