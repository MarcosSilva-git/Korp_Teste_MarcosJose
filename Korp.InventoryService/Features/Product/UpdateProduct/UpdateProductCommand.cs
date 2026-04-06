using Korp.InventoryService.Shared.DTOs.Product.GetProducts;
using Korp.Shared.Abstractions;
using Korp.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Korp.InventoryService.Features.Product.UpdateProduct;

public record UpdateProductCommand(int Id, string Name, int Stock) : IRequest<Result<GetProductsResponse, ValidationResult>>;
