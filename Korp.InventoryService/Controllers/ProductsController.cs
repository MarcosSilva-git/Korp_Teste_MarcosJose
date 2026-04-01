using Korp.InventoryService.Data;
using Korp.InventoryService.DTOs;
using Korp.InventoryService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Korp.InventoryService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : Controller
{
    private readonly ILogger<ProductsController> _logger;
    private readonly InventoryDbContext _inventoryDbContext;

    public ProductsController(ILogger<ProductsController> logger, InventoryDbContext inventoryDbContext)
    {
        _logger = logger;
        _inventoryDbContext = inventoryDbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _inventoryDbContext
            .Products
            .AsNoTracking()
            .ToListAsync();

        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddProductRequest product)
    {
        var newProduct = new ProductEntity
        {
            Name = product.Name,
            Balance = product.Balance
        };

        _inventoryDbContext.Add(newProduct);
        await _inventoryDbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(Add), newProduct);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProductRequest updatedProduct)
    {
        if (id <= 0)
        {
            return Problem(
                title: "Product not found",
                detail: $"Product with id {id} was not found.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        var product = await _inventoryDbContext.Products.FindAsync(id);

        if (product is null)
        {
            return Problem(
                title: "Product not found",
                detail: $"Product with id {id} was not found.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        product.Name = updatedProduct.Name;
        product.Balance = updatedProduct.Balance;

        await _inventoryDbContext.SaveChangesAsync();

        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
        {
            return Problem(
                title: "Product not found",
                detail: $"Product with id {id} was not found.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        var product = await _inventoryDbContext.Products.FindAsync(id);

        if (product is null)
        {
            return Problem(
                title: "Product not found",
                detail: $"Product with id {id} was not found.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        product.DeletedAt = DateTime.UtcNow;

        await _inventoryDbContext.SaveChangesAsync();

        return NoContent();
    }
}
