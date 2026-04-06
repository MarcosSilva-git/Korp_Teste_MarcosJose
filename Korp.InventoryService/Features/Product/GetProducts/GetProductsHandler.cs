using Korp.InventoryService.Infraestructure;
using Korp.InventoryService.Shared.DTOs.Product.GetProducts;
using Korp.Shared.Abstractions;
using Korp.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Korp.InventoryService.Features.Product.GetProducts;

public class GetProductsHandler(
    InventoryDbContext _inventoryDbContext, 
    ILogger<GetProductsHandler> _logger) : IRequestHandlerAsync<GetProductsRequest, Result<IEnumerable<GetProductsResponse>, ValidationResult>>
{

    public async Task<Result<IEnumerable<GetProductsResponse>, ValidationResult>> HandleAsync(GetProductsRequest request, CancellationToken ct)
    {
        if (request.Ids is null || string.IsNullOrWhiteSpace(request.Ids))
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
                .ToListAsync(ct);
        }

        SortedSet<int> idSet;

        try
        {
            idSet = [.. request.Ids.Split(',').Select(int.Parse)];
        }
        catch (FormatException e)
        {
            _logger.LogWarning(e, "Failed to parse IDs from query string: {Ids}", request.Ids);
            return new ValidationResult("One or more IDs could not be converted to integers.", [nameof(request.Ids)]);
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
            .ToListAsync(ct);
    }
}
