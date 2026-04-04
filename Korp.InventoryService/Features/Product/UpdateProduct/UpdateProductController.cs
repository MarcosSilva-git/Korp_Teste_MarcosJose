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

        if (result.IsFailure)
        {
            var memberName = result.Error!.MemberNames.FirstOrDefault() ?? string.Empty;
            ModelState.AddModelError(memberName, result.Error!.ErrorMessage ?? string.Empty);

            return ValidationProblem(ModelState);
        }

        return Ok(result.Value);
    }
}
