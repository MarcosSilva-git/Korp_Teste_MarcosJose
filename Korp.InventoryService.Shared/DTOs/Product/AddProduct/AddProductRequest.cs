using Korp.InventoryService.Shared.DTOs.Product.GetProducts;
using Korp.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Korp.InventoryService.Shared.DTOs.Product.AddProduct;

public class AddProductRequest : IRequest<GetProductsResponse>
{
    [MinLength(1)]
    public string Name { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "{0} should be greater then zero")]
    public int Stock { get; set; }
}
