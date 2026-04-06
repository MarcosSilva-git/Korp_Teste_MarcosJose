using Korp.InventoryService.Features.Product.AddProduct;
using Korp.InventoryService.Features.Product.DeleteProduct;
using Korp.InventoryService.Features.Product.GetProducts;
using Korp.InventoryService.Features.Product.ReserveProducts;
using Korp.InventoryService.Features.Product.UpdateProduct;
using Korp.InventoryService.Shared.DTOs.Product.AddProduct;
using Korp.InventoryService.Shared.DTOs.Product.CommitReservedProducts;
using Korp.InventoryService.Shared.DTOs.Product.DeleteProduct;
using Korp.InventoryService.Shared.DTOs.Product.GetProducts;
using Korp.InventoryService.Shared.DTOs.Product.ReserveProducts;
using Korp.InventoryService.Shared.DTOs.Product.UpdateProduct;
using Korp.Shared;
using Korp.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Korp.InventoryService.Features.Product;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IDispatcher _dispatcher) : SharedControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? ids, CancellationToken ct)
    {
        var result = await _dispatcher.SendAsync(new GetProductsRequest(ids), ct);

        if (result.IsFailure)
            return CreateValidationProblemFromResult(result);

        return Ok(new { Data = result.Value });
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddProductRequest product, CancellationToken ct)
    {
        var newProduct = await _dispatcher.SendAsync(product, ct);
        return CreatedAtAction(nameof(Add), new { newProduct.Id }, newProduct);
    }

    [HttpPost("reserve")]
    public async Task<IActionResult> ReserveProducts([FromBody] ReserveProductsRequest request, CancellationToken ct)
    {
        var result = await _dispatcher.SendAsync(request, ct);

        if (result.IsFailure)
            return CreateValidationsProblemFromResult(result);

        return NoContent();
    }

    [HttpPost("reserve/rollback")]
    public async Task<IActionResult> RollbackReserveProducts(RollbackReserveProductsRequest request, CancellationToken ct)
    {
        await _dispatcher.SendAsync(request, ct);
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProductRequest request, CancellationToken ct)
    {
        var result = await _dispatcher.SendAsync(new UpdateProductCommand(id, request.Name, request.Stock), ct);

        if (result.IsFailure)
            return CreateValidationProblemFromResult(result);

        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await _dispatcher.SendAsync(new DeleteProductRequest(id), ct);

        if (result.IsFailure)
            return CreateValidationProblemFromResult(result);

        return NoContent();
    }

    [HttpPost("reserve/commit")]
    public async Task<IActionResult> Commit(CommitReservedProductsRequest request, CancellationToken ct)
    {
        var result = await _dispatcher.SendAsync(request, ct);

        if (result.IsFailure)
            return CreateValidationProblemFromResult(result);

        return NoContent();
    }
}
