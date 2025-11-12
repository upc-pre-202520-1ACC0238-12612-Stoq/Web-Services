using Lot.Inventaries.Domain.Model.Commands;
using Lot.Inventaries.Domain.Model.ValueOjbects;
using Lot.Inventaries.Interfaces.REST.Resources;

namespace Lot.Inventaries.Interfaces.REST.Transform;

/// <summary>
/// Assembler para transformar Resources a Commands.
/// Convierte Resource a Command usando foreign key (ProductoId).
/// </summary>
public static class CreateInventoryByProductCommandAssembler
{
    public static CreateInventoryByProductCommand ToCommandFromResource(CreateInventoryByProductResource resource)
    {
        return new CreateInventoryByProductCommand(
            productoId: resource.ProductoId,
            // ❌ fechaEntrada eliminada - ahora es automática
            cantidad: new Cantidad(resource.Cantidad),
            precio: new Precio(resource.Precio),
            stockMinimo: new StockMinimo(resource.StockMinimo)
        );
    }
}