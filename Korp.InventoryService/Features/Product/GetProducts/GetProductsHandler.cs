using Korp.InventoryService.Infraestructure;
using Korp.InventoryService.Shared.DTOs.GetProducts;
using Korp.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Korp.InventoryService.Features.Product.GetProducts;

public class GetProductsHandler(InventoryDbContext inventoryDbContext, ILogger<GetProductsHandler> logger)
{
    private readonly InventoryDbContext _inventoryDbContext = inventoryDbContext;
    private readonly ILogger<GetProductsHandler> _logger = logger;

    public async Task<Result<IEnumerable<GetProductsResponse>, ValidationResult>> HandlerAsync(string? ids, CancellationToken cancellationToken)
    {
        if (ids is null || string.IsNullOrWhiteSpace(ids))
        {
            return await _inventoryDbContext
                .Products
                .AsNoTracking()
                .Select(p => new GetProductsResponse()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Stock = p.Stock,
                    Reserved = p.Reserved,
                    Available = p.Available,
                    CreatedAt = p.CreatedAt,
                })
                .ToListAsync(cancellationToken);
        }

        SortedSet<int> idSet;

        try
        {
            idSet = [.. ids.Split(',').Select(int.Parse)];
        }
        catch (FormatException e)
        {
            _logger.LogWarning(e, "Failed to parse IDs from query string: {Ids}", ids);
            return new ValidationResult("One or more IDs could not be converted to integers.", [nameof(ids)]);
        }

        return await _inventoryDbContext
            .Products
            .AsNoTracking()
            .Where(p => idSet.Contains(p.Id))
            .Select(p => new GetProductsResponse()
            {
                Id = p.Id,
                Name = p.Name,
                Stock = p.Stock,
                Reserved = p.Reserved,
                Available = p.Available,
                CreatedAt = p.CreatedAt,
            })
            .ToListAsync(cancellationToken);
    }
}
