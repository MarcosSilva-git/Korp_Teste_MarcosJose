using Korp.InventoryService.Features.Product.Domain.Enums;
using Korp.InventoryService.Infraestructure;
using Korp.InventoryService.Shared.DTOs.Product.ReserveProducts;
using Korp.Shared.Abstractions;
using Korp.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Korp.InventoryService.Features.Product.ReserveProducts;

public class RollbackReserveProductsHandler(
    InventoryDbContext _inventoryDbContext,
    ILogger<RollbackReserveProductsHandler> _logger) : IRequestHandlerAsync<RollbackReserveProductsRequest, Result>
{
    public async Task<Result> HandleAsync(RollbackReserveProductsRequest request, CancellationToken ct)
    {
        var reservations = await _inventoryDbContext.ReservedProducts
            .Include(r => r.Product)
            .Where(r => r.SagaId == request.SagaId)
            .ToListAsync(ct);

        if (reservations.Count == 0)
        {
            _logger.LogWarning("Rollback solicitado para SagaId {SagaId}, mas nenhuma reserva foi encontrada.", request.SagaId);
            return Result.Success();
        }

        foreach (var reservation in reservations.Where(r => r.StockMovementStatus == StockMovementStatusEnum.Reserved))
        {
            reservation.Product!.RemoveReservation(reservation.Quantity);
            _inventoryDbContext.Remove(reservation);
        }
        
        await _inventoryDbContext.SaveChangesAsync(ct);
        return Result.Success();
    }   
}
