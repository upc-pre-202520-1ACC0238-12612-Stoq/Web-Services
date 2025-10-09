namespace Lot.ProductManagement.Domain.Model.Queries;

/// <summary>
/// Get Products By Tag Query
/// </summary>
/// <param name="TagId">
/// El identificador de la etiqueta para filtrar los productos.
/// </param>
public record GetProductsByTagQuery(int TagId); 