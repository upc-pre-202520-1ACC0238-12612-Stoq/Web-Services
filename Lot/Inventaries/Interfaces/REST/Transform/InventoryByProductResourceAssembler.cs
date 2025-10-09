//L
using Lot.Inventaries.Domain.Model.Aggregates;
using Lot.Inventaries.Interfaces.REST.Resources;

namespace Lot.Inventaries.Interfaces.REST.Transform;

public static class InventoryByProductResourceAssembler
{
    public static InventoryByProductResource ToCommandFromResource(InventoryByProduct entity)
    {
        return new InventoryByProductResource
        {
            Id = entity.Id,
            Categoria = entity.Categoria,
            Producto = entity.Producto,
            FechaEntrada = entity.FechaEntrada,
            Cantidad = entity.Cantidad,
            Precio = entity.Precio,
            StockMinimo = entity.StockMinimo,
            UnidadMedida = entity.UnidadMedida
        };

    }
}