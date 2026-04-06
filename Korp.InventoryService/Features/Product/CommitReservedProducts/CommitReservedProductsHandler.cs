using Korp.InventoryService.Features.Product.Domain.Exceptions;
using Korp.InventoryService.Infraestructure;
using Korp.InventoryService.Shared.DTOs.Product.CommitReservedProducts;
using Korp.Shared.Abstractions;
using Korp.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Korp.InventoryService.Features.Product.CommitReservedProducts;

public class CommitReservedProductsHandler(
    InventoryDbContext _inventoryDbContext,
    ILogger<CommitReservedProductsHandler> _logger) : IRequestHandlerAsync<CommitReservedProductsRequest, Result<CommitReservedProductsResponse, ValidationResult>>
{
    public async Task<Result<CommitReservedProductsResponse, ValidationResult>> HandleAsync(CommitReservedProductsRequest command, CancellationToken ct)
    {
        var reservedProducts = await _inventoryDbContext.ReservedProducts
            .Include(rp => rp.Product)
            .Where(rp => rp.SagaId == command.SagaId)
            .ToListAsync(ct);

        if (reservedProducts.Count == 0)
            return new ValidationResult("No reserved products found for the given SagaId", [ $"SagaId_{command.SagaId}" ]);

        var validationErrors = new List<ValidationResult>();

        foreach (var reservedProduct in reservedProducts)
        {
            if (reservedProduct.Product is null)
            {
                var errorMessage = $"Product with ID {reservedProduct.ProductId} not found for reserved product with ID {reservedProduct.Id}";
                _logger.LogCritical(errorMessage);
                validationErrors.Add(new (errorMessage, [ $"ProductId_{reservedProduct.ProductId}" ]));

                continue;
            }

            reservedProduct.MarkAsCommitted();
            reservedProduct.Product.DeleteReservation(reservedProduct.Quantity);
        }

        if (validationErrors.Count > 0)
        {
            var summary = string.Join(" | ", validationErrors.Select(x => x.ErrorMessage));
            throw new InconsistentSystemStateException(summary);
        }
        
        await _inventoryDbContext.SaveChangesAsync(ct);

        return new CommitReservedProductsResponse(reservedProducts.Count);
    }
}
