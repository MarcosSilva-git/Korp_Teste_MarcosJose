namespace Korp.InventoryService.Infraestructure;

public abstract class EntityBase
{
    public int Version { get; private set => field = value + 1; } = default!;
}
