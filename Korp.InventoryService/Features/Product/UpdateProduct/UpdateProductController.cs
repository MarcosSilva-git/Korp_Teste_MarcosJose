using Korp.InventoryService.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Korp.InventoryService.Features.Product.UpdateProduct;

[ApiController]
public class UpdateProductController(UpdateProductHandler updateProductHandler) : ControllerBase
{
    private readonly UpdateProductHandler _updateProductHandler = updateProductHandler;

    [HttpPut("api/products/{id}")]
    public async Task<IActionResult> Update(int id, UpdateProductRequest updatedProduct)
    {
        var result = await _updateProductHandler.HandlerAsync(id, updatedProduct);

        if (result.IsFailure && result.Error is KeyNotFoundException)
        {
            return Problem(
                    title: "Product not found",
                    detail: result.Error.Message,
                    statusCode: StatusCodes.Status404NotFound
                );
        }

        if (result.IsFailure)
        {
            throw new InvalidOperationException("Unhandled result exception", result.Error);
        }

        return Ok(result.Value);
    }
}
