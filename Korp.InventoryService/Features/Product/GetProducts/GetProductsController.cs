using Korp.InventoryService.Infraestructure;
using Microsoft.AspNetCore.Mvc;

namespace Korp.InventoryService.Features.Product.GetProducts;

public class GetProductsController(GetProductsHandler getProductsHandler) : ControllerBase
{
    private readonly GetProductsHandler _getProductsHandler = getProductsHandler;

    [HttpGet("api/products")]
    public async Task<IActionResult> Get([FromQuery] string? ids, CancellationToken cancellationToken)
    {
        var result = await _getProductsHandler.HandlerAsync(ids, cancellationToken);

        if (result.IsFailure)
        {
            var memberName = result.Error!.MemberNames.FirstOrDefault() ?? string.Empty;
            ModelState.AddModelError(memberName, result.Error.ErrorMessage ?? string.Empty);

            return ValidationProblem(ModelState);
        }

        return Ok(new { Products = result.Value });
    }
}
