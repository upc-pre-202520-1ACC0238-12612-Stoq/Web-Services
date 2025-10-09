//L
using Lot.Inventaries.Domain.Model.Commands;
using Lot.Inventaries.Domain.Model.ValueOjbects;
using Lot.Inventaries.Interfaces.REST.Resources;

namespace Lot.Inventaries.Interfaces.REST.Transform;

public static class CreateInventoryByBatchCommandAssembler
{
    public static CreateInventoryByBatchCommand ToCommand(CreateInventoryByBatchResource resource)
    {
        return new CreateInventoryByBatchCommand(
            resource.Proveedor,
            resource.Producto,
            resource.FechaEntrada,
            new Cantidad(resource.Cantidad),
            new Precio(resource.Precio),
            new Unidad(resource.Unidad)
        );
    }
}