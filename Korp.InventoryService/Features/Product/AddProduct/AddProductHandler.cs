using Korp.InventoryService.Features.Product.Domain;
using Korp.InventoryService.Infraestructure;
using Korp.InventoryService.Shared.DTOs.Product.AddProduct;
using Korp.InventoryService.Shared.DTOs.Product.GetProducts;
using Korp.Shared.Interfaces;

namespace Korp.InventoryService.Features.Product.AddProduct;

public class AddProductHandler(InventoryDbContext _inventoryDbContext)
    : IRequestHandlerAsync<AddProductRequest, GetProductsResponse>
{
    public async Task<GetProductsResponse> HandleAsync(AddProductRequest request, CancellationToken ct)
    {
        var newProduct = new ProductEntity(request.Name, request.Stock);

        _inventoryDbContext.Add(newProduct);
        await _inventoryDbContext.SaveChangesAsync(ct);

        return new()
        {
            Id = newProduct.Id,
            Name = newProduct.Name,
            Stock = newProduct.Stock,
            Reserved = newProduct.Reserved,
            Available = newProduct.Available,
            CreatedAt = newProduct.CreatedAt
        };
    }
}
