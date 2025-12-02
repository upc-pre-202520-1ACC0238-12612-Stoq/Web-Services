using Lot.Sales.Domain.Model.Aggregates;
using Lot.Sales.Interfaces.REST.Resources;

namespace Lot.Sales.Interfaces.REST.Transform;

/// <summary>
/// Assembler para transformar Entities a Resources enriquecidos de ventas.
/// Convierte Entity a Resource con datos calculados y enriquecidos.
/// </summary>
public static class SaleResourceAssembler
{
    public static SaleResource ToResourceFromEntity(Sale entity)
    {
        return new SaleResource
        {
            // Campos existentes de ventas
            Id = entity.Id,
            ProductId = entity.ProductId,
            ProductName = entity.ProductName,
            CategoryName = entity.CategoryName,
            SaleDate = entity.SaleDate,
            Quantity = entity.Quantity.Value,
            UnitPrice = entity.UnitPrice.Value,
            TotalAmount = entity.TotalAmount,
            CustomerName = entity.CustomerName,
            Notes = entity.Notes,
            SaleType = entity.GetSaleType(),

            // ⭐ NUEVOS campos para soporte de combos
            ComboId = entity.ComboId,
            ComboName = entity.ComboName,
            IsComboSale = entity.IsComboSale
        };
    }
}