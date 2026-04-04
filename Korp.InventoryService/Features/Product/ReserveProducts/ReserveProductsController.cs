using Korp.InventoryService.Features.Product.ReserveProducts;
using Korp.InventoryService.Shared.DTOs.ReserveProducts;
using Microsoft.AspNetCore.Mvc;

namespace Korp.InventoryService.Features.Product.ReserveProducts;

[ApiController]
public class ReserveProductsController(ReserveProductsHandler reserveProductsHandler) : ControllerBase
{
    private readonly ReserveProductsHandler _reserveProductsHandler = reserveProductsHandler;

    [HttpPost("api/products/reserve")]
    public async Task<IActionResult> ReserveProducts([FromBody] ReserveProductsRequest request)
    {
        var result = await _reserveProductsHandler.HandleAsync(request);

        if (result.IsFailure)
        {
            foreach (var error in result.Error!)
            {
                var memberName = error.MemberNames.FirstOrDefault() ?? string.Empty;
                ModelState.AddModelError(memberName, error.ErrorMessage ?? string.Empty);
            }

            return ValidationProblem(ModelState);
        }

        return Ok();
    }
}
