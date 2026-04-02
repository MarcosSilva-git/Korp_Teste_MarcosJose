using Korp.InventoryService.Shared.DTOs.AddProduct;
using Microsoft.AspNetCore.Mvc;

namespace Korp.InventoryService.Features.Product.AddProduct;

[ApiController]
public class AddProductController(AddProductHandler addProductHandler) : Controller
{
    private readonly AddProductHandler _addProductHandler = addProductHandler;

    [HttpPost("api/products")]
    public async Task<IActionResult> Add(AddProductRequest product)
    {
        var newProduct = await _addProductHandler.HandleAsync(product);
        return CreatedAtAction(nameof(Add), new { newProduct.Id }, newProduct);
    }
}
