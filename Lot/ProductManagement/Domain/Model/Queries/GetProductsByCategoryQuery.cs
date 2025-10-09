namespace Lot.ProductManagement.Domain.Model.Queries;

/// <summary>
/// Get Products By Category Query
/// </summary>
/// <param name="CategoryId">
/// El identificador de la categoría para filtrar los productos.
/// </param>
public record GetProductsByCategoryQuery(int CategoryId); 