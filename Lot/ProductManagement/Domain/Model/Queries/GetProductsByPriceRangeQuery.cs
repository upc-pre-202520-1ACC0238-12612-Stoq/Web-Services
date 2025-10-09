namespace Lot.ProductManagement.Domain.Model.Queries;

/// <summary>
/// Get Products By Price Range Query
/// </summary>
/// <param name="MinPrice">
/// El precio mínimo para filtrar los productos.
/// </param>
/// <param name="MaxPrice">
/// El precio máximo para filtrar los productos.
/// </param>
public record GetProductsByPriceRangeQuery(decimal MinPrice, decimal MaxPrice); 