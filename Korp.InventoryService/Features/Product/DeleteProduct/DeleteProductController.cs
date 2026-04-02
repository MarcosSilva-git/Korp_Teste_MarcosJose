using Korp.InventoryService.Infraestructure;
using Microsoft.AspNetCore.Mvc;

namespace Korp.InventoryService.Features.Product.DeleteProduct;

[ApiController]
public class DeleteProductController(DeleteProductHandler deleteProductHandler) : ControllerBase
{
    private readonly DeleteProductHandler _deleteProductHandler = deleteProductHandler;

    [HttpDelete("api/products/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _deleteProductHandler.HandleAsync(id);

        if (result.IsFailure && result.Error is KeyNotFoundException)
        {
            return Problem(
                title: "Product not found",
                detail: result.Error!.Message,
                statusCode: StatusCodes.Status404NotFound
            );
        }

        if (result.IsFailure)
        {
            throw new InvalidOperationException("Unhandled result exception", result.Error);
        }

        return NoContent();
    }
}
