using Korp.InventoryService.Features.Product.Domain;
using Korp.InventoryService.Infraestructure;
using Korp.InventoryService.Shared.DTOs.AddProduct;
using Korp.InventoryService.Shared.DTOs.GetProducts;

namespace Korp.InventoryService.Features.Product.AddProduct;

public class AddProductHandler(InventoryDbContext inventoryDbContext)
{
    private readonly InventoryDbContext _inventoryDbContext = inventoryDbContext;

    public async Task<GetProductsResponse> HandleAsync(AddProductRequest request)
    {
        var newProduct = new ProductEntity(request.Name, request.Stock);

        _inventoryDbContext.Add(newProduct);
        await _inventoryDbContext.SaveChangesAsync();

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
