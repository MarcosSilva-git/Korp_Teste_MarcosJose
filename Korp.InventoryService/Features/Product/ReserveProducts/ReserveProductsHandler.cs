using Korp.InventoryService.Features.Product.Domain;
using Korp.InventoryService.Infraestructure;
using Korp.InventoryService.Shared.DTOs.ReserveProducts;
using Korp.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Korp.InventoryService.Features.Product.ReserveProducts;

public class ReserveProductsHandler(InventoryDbContext inventoryDbContext, ILogger<ReserveProductsHandler> logger)
{
    private readonly InventoryDbContext _inventoryDbContext = inventoryDbContext;
    private readonly ILogger<ReserveProductsHandler> _logger = logger;

    public async Task<Result<bool, List<ValidationResult>>> HandleAsync(ReserveProductsRequest request)
    {
        var ids = new int[request.Products.Count];

        foreach (var item in request.Products)
            ids.Append(item.Id);

        var products = await GetProductsAsync(ids);

        var validationErrors = ValidateIdsExistence(ids, products);
        if (validationErrors.Any()) 
            return validationErrors;

        var stockErrors = ValidateStockAvailability(request.Products, products);
        if (stockErrors.Any()) 
            return stockErrors;

        await UpsertReservedProductsAsync(request, products);

        try
        {
            await _inventoryDbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
            _logger.LogError("Concurrency error while reserving products: {Message}", e.Message);
            throw;
        }

        return true;
    }

    private async Task<List<ProductEntity>> GetProductsAsync(int[] ids)
    {
        return await _inventoryDbContext.Products
            .Where(p => ids.Contains(p.Id))
            .AsNoTracking()
            .ToListAsync();
    }

    private List<ValidationResult> ValidateIdsExistence(int[] requestedIds, List<ProductEntity> products)
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

    private async Task UpsertReservedProductsAsync(ReserveProductsRequest request, List<ProductEntity> products)
    {
        var existingReservations = await _inventoryDbContext.ReservedProducts
            .Where(ap => ap.SagaId == request.Sagaid)
            .ToListAsync();

        foreach (var reqItem in request.Products)
        {
            var reservation = existingReservations.Find(r => r.ProductId == reqItem.Id);
            var product = products.First(p => p.Id == reqItem.Id);

            if (reservation is null)
            {
                _inventoryDbContext.ReservedProducts.Add(new ReservedProductEntity(
                    reqItem.Id,
                    request.Sagaid,
                    reqItem.Quantity,
                    request.OriginId,
                    request.OriginType));

                product.AddReservation(reqItem.Quantity);
            }
            else
            {
                reservation.UpdateQuantity(reqItem.Quantity + reservation.Quantity);
                product.RemoveReservation(reqItem.Quantity);
            }
        }
    }
}
