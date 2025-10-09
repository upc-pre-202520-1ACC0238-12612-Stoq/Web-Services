using Lot.Inventaries.Domain.Model.Commands;
using Lot.Inventaries.Domain.Model.ValueOjbects;
using Lot.Inventaries.Interfaces.REST.Resources;

namespace Lot.Inventaries.Interfaces.REST.Transform;

public static class CreateInventoryByProductCommandAssembler
{
    public static CreateInventoryByProductCommand ToCommandFromResource(CreateInventoryByProductResource resource)
    {
        return new CreateInventoryByProductCommand(
            resource.Categoria,
            resource.Producto,
            resource.FechaEntrada,
            new Cantidad(resource.Cantidad),
            new Precio(resource.Precio),
            new StockMinimo(resource.StockMinimo),
            new Unidad(resource.UnidadMedida)
        );
    }
}