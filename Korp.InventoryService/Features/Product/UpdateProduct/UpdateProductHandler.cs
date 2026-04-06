using Korp.InventoryService.Infraestructure;
using Korp.InventoryService.Shared.DTOs.Product.GetProducts;
using Korp.InventoryService.Shared.DTOs.Product.UpdateProduct;
using Korp.Shared.Abstractions;
using Korp.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Korp.InventoryService.Features.Product.UpdateProduct;

public class UpdateProductHandler(InventoryDbContext _inventoryDbContext)
    : IRequestHandlerAsync<UpdateProductCommand, Result<GetProductsResponse, ValidationResult>>
{
    public async Task<Result<GetProductsResponse, ValidationResult>> HandleAsync(UpdateProductCommand command, CancellationToken ct)
    {
        var product = await _inventoryDbContext.Products.FindAsync(command.Id);

        if (product is null)
            return new ValidationResult($"Product with id {command.Id} was not found.", [$"ProductId_{command.Id}"]);

        product.ChangeName(command.Name);

        if (product.Stock == 0)
        {
            product.UpdateStock(command.Stock);
        }
        else if (command.Stock < product.Stock && command.Stock < product.Reserved)
        {
            return new ValidationResult(
                $"The requested quantity exceeds the current available balance for this product. Available: {product.Available}, Requested: {command.Stock}", 
                [$"ProductId_{command.Id}"]);
        }
        else
        {
            throw new InvalidOperationException(
                $"Invalid stock transition for ProductId: {command.Id}. " +
                $"Available: {product.Available}, " +
                $"Reserved: {product.Reserved}, " +
                $"Requested: {command.Stock}");
        }

        await _inventoryDbContext.SaveChangesAsync(ct);

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
