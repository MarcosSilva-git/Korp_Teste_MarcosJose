using Korp.InventoryService.Infraestructure;
using Microsoft.AspNetCore.Mvc;

namespace Korp.InventoryService.Features.Product.GetProducts;

public class GetProductsController(GetProductsHandler getProductsHandler) : ControllerBase
{
    private readonly GetProductsHandler _getProductsHandler = getProductsHandler;

    [HttpGet("api/products")]
    public async Task<IActionResult> Get([FromQuery] string? ids = null)
    {
        var result = await _getProductsHandler.HandlerAsync(ids);

        if (result.IsFailure && result.Error is FormatException)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid format for product IDs",
                detail: result.Error.Message
            );
        }

        return Ok(new { Products = result.Value });
    }
}
