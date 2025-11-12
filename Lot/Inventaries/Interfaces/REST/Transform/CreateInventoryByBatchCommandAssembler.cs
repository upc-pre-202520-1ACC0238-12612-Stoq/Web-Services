//L
using Lot.Inventaries.Domain.Model.Commands;
using Lot.Inventaries.Domain.Model.ValueOjbects;
using Lot.Inventaries.Interfaces.REST.Resources;

namespace Lot.Inventaries.Interfaces.REST.Transform;

/// <summary>
/// Assembler para transformar Resources a Commands.
/// Convierte Resource a Command usando foreign key (ProductoId).
/// </summary>
public static class CreateInventoryByBatchCommandAssembler
{
    public static CreateInventoryByBatchCommand ToCommandFromResource(CreateInventoryByBatchResource resource)
    {
        return new CreateInventoryByBatchCommand(
            productoId: resource.ProductoId,
            proveedor: resource.Proveedor,
            unidadId: resource.UnidadId,
            // ❌ fechaEntrada eliminada - ahora es automática
            cantidad: new Cantidad(resource.Cantidad),
            precio: new Precio(resource.Precio)
        );
    }
}