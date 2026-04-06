namespace Korp.InventoryService.Features.Product.Domain.Exceptions;

public class InconsistentSystemStateException : Exception
{
    public InconsistentSystemStateException(string message) : base(message) { }
}
