using Korp.InventoryService.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace Korp.InventoryService.Features.Product.ReserveProducts;

public class RollbackReserveProductsHandler(InventoryDbContext inventoryDbContext)
{
    private readonly InventoryDbContext _inventoryDbContext = inventoryDbContext;

    public async Task HandleAsync(Guid sagaId)
    {
        var reservations = await _inventoryDbContext.ReservedProducts
            .Where(r => r.SagaId == sagaId && r.RolledbackAt == null)
            .ToListAsync();

        if (reservations.Any())
        {
            var products = await _inventoryDbContext.Products
                .Where(p => reservations.Select(r => r.ProductId).Contains(p.Id))
                .ToListAsync();

            foreach (var reservation in reservations)
            {
                var product = products.First(p => p.Id == reservation.ProductId);

                product.RemoveReservation(reservation.Quantity);
                reservation.RolledbackAt = DateTime.UtcNow; 
            }

            await _inventoryDbContext.SaveChangesAsync();
        }
    }   
}
