using Lot.Sales.Domain.Model.Commands;
using Lot.Sales.Interfaces.REST.Resources;

namespace Lot.Sales.Interfaces.REST.Transform;

/// <summary>
/// Assembler para transformar Resources a Commands de ventas.
/// Convierte Resource a Command usando los datos esenciales de la venta.
/// Soporta ventas individuales y ventas de combos.
/// </summary>
public static class CreateSaleCommandAssembler
{
    public static CreateSaleCommand ToCommandFromResource(CreateSaleResource resource)
    {
        return new CreateSaleCommand(
            // Campos existentes para ventas normales
            resource.ProductId,
            resource.Quantity,
            resource.UnitPrice,
            resource.CustomerName,
            resource.Notes,

            // ⭐ NUEVOS campos para soporte de combos
            resource.ComboId,
            resource.ComboName
        );
    }
}