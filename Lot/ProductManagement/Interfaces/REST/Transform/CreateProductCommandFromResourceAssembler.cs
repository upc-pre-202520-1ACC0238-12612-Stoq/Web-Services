using Lot.ProductManagement.Domain.Model.Commands;
using Lot.ProductManagement.Interfaces.REST.Resources;

namespace Lot.ProductManagement.Interfaces.REST.Transform;

/// <summary>
/// Assembler para convertir de CreateProductResource a CreateProductCommand
/// </summary>
public static class CreateProductCommandFromResourceAssembler
{
    /// <summary>
    /// Convierte un CreateProductResource a CreateProductCommand
    /// </summary>
    /// <param name="resource">El recurso de crear producto</param>
    /// <returns>El comando de crear producto</returns>
    public static CreateProductCommand ToCommandFromResource(CreateProductResource resource)
    {
        return new CreateProductCommand(
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