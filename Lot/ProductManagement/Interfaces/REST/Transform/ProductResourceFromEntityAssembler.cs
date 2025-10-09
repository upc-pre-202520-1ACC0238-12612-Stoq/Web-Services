using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.ProductManagement.Interfaces.REST.Resources;

namespace Lot.ProductManagement.Interfaces.REST.Transform;

/// <summary>
/// Assembler para convertir de Product a ProductResource
/// </summary>
public static class ProductResourceFromEntityAssembler
{
    /// <summary>
    /// Convierte un Product a ProductResource
    /// </summary>
    /// <param name="entity">La entidad producto</param>
    /// <returns>El recurso de producto</returns>
    public static ProductResource ToResourceFromEntity(Product entity)
    {
        var tags = entity.ProductTags?.Select(pt => new TagResource(
            pt.Tag?.Id ?? 0,
            pt.Tag?.Name ?? string.Empty
        )).ToList() ?? new List<TagResource>();

        return new ProductResource(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.PurchasePrice,
            entity.SalePrice,
            entity.InternalNotes,
            entity.CategoryId,
            entity.Category?.Name ?? string.Empty,
            entity.UnitId,
            entity.Unit?.Name ?? string.Empty,
            entity.Unit?.Abbreviation ?? string.Empty,
            tags
        );
    }
} 