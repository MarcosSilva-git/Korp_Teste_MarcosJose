using Korp.Shared.Abstractions;
using Korp.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Korp.InventoryService.Shared.DTOs.Product.GetProducts;

public record GetProductsRequest(string? Ids) : IRequest<Result<IEnumerable<GetProductsResponse>, ValidationResult>>;
