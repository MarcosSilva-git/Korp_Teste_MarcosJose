using Korp.Shared.Attributes;

namespace Korp.InventoryService.Shared.DTOs.ReserveProducts;

public class RollbackReserveProductsRequest
{
    [NotEmptyGuid]
    public Guid SagaId { get; set; }
}
