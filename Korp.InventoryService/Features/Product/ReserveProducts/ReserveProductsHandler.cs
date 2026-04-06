using Korp.InventoryService.Features.Product.Domain;
using Korp.InventoryService.Infraestructure;
using Korp.InventoryService.Shared.DTOs.Product.ReserveProducts;
using Korp.Shared.Abstractions;
using Korp.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Korp.InventoryService.Features.Product.ReserveProducts;

public class ReserveProductsHandler(
    InventoryDbContext _inventoryDbContext, 
    ILogger<ReserveProductsHandler> _logger) : IRequestHandlerAsync<ReserveProductsRequest, Result<bool, List<ValidationResult>>>
{
    public async Task<Result<bool, List<ValidationResult>>> HandleAsync(ReserveProductsRequest request, CancellationToken _)
    {
        var ids = new List<int>(request.Products.Count);

        foreach (var item in request.Products)
            ids.Add(item.Id);

        var products = await GetProductsAsync(ids);

        // await Task.Delay(2_000);

        var validationErrors = ValidateIdsExistence(ids, products);
        if (validationErrors.Any()) 
            return validationErrors;

        var stockErrors = ValidateStockAvailability(request.Products, products);
        if (stockErrors.Any()) 
            return stockErrors;

        var reservationWasMade = await ReserveProductsAsync(request, products);

        try
        {
            if (reservationWasMade)
                await _inventoryDbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
            _logger.LogError("Concurrency error while reserving products: {Message}", e.Message);
            throw;
        }

        return true;
    }

    private async Task<List<ProductEntity>> GetProductsAsync(List<int> ids)
    {
        return await _inventoryDbContext.Products
            .Where(p => ids.Contains(p.Id))
            .ToListAsync();
    }

    private List<ValidationResult> ValidateIdsExistence(List<int> requestedIds, List<ProductEntity> products)
    {
        var foundIds = products.Select(p => p.Id);
        var missingIds = requestedIds.Except(foundIds).ToList();

        return missingIds
            .Select(id => new ValidationResult($"Product with ID {id} not found.", [$"ProductId_{id}"]))
            .ToList();
    }

    private List<ValidationResult> ValidateStockAvailability(List<ReserveProductRequest> requested, List<ProductEntity> products)
    {
        var errors = new List<ValidationResult>();

        foreach (var product in products)
        {
            var req = requested.First(p => p.Id == product.Id);
            if (product.Available < req.Quantity)
            {
                errors.Add(new ValidationResult(
                    $"Insufficient stock for Product ID {product.Id}. Available: {product.Stock}.",
                    [$"ProductId_{product.Id}"]));
            }
        }

        return errors;
    }

    /// <returns>A boolean value indicating whether a reservation has been made or not.</returns>
    private async Task<bool> ReserveProductsAsync(ReserveProductsRequest request, List<ProductEntity> products)
    {
        var existingReservations = await _inventoryDbContext.ReservedProducts
            .Where(ap => ap.SagaId == request.Sagaid)
            .FirstOrDefaultAsync();

        if (existingReservations is not null)
        {
            _logger.LogWarning("Existing reservation found for Saga ID {SagaId}. Skipping reservation.", request.Sagaid);
            return false;
        }

        foreach (var reqItem in request.Products)
        {
            var product = products.First(p => p.Id == reqItem.Id);

            _inventoryDbContext.ReservedProducts.Add(new ReservedProductEntity(
                reqItem.Id,
                request.Sagaid,
                reqItem.Quantity,
                request.OriginId,
                request.OriginType));

            product.AddReservation(reqItem.Quantity);
        }

        return true;
    }
}
