using Lot.ProductManagement.Domain.Model.Commands;
using Lot.ProductManagement.Interfaces.REST.Resources;

namespace Lot.ProductManagement.Interfaces.REST.Transform;

/// <summary>
/// Assembler para convertir de UpdateProductResource a UpdateProductCommand
/// </summary>
public static class UpdateProductCommandFromResourceAssembler
{
    /// <summary>
    /// Convierte un UpdateProductResource a UpdateProductCommand
    /// </summary>
    /// <param name="id">El identificador del producto</param>
    /// <param name="resource">El recurso de actualizar producto</param>
    /// <returns>El comando de actualizar producto</returns>
    public static UpdateProductCommand ToCommandFromResource(int id, UpdateProductResource resource)
    {
        return new UpdateProductCommand(
            id,
            resource.Name,
            resource.Description,
            resource.PurchasePrice,
            resource.SalePrice,
            resource.InternalNotes,
            resource.CategoryId,
            resource.UnitId,
            resource.TagIds
        );
    }
} 