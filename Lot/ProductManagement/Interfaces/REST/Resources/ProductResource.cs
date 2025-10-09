namespace Lot.ProductManagement.Interfaces.REST.Resources;

/// <summary>
/// Recurso de producto para la API REST
/// </summary>
/// <param name="Id">
/// Identificador único del producto
/// </param>
/// <param name="Name">
/// Nombre del producto
/// </param>
/// <param name="Description">
/// Descripción del producto
/// </param>
/// <param name="PurchasePrice">
/// Precio de compra del producto
/// </param>
/// <param name="SalePrice">
/// Precio de venta del producto
/// </param>
/// <param name="InternalNotes">
/// Notas internas del producto
/// </param>
/// <param name="CategoryId">
/// Identificador de la categoría
/// </param>
/// <param name="CategoryName">
/// Nombre de la categoría
/// </param>
/// <param name="UnitId">
/// Identificador de la unidad de medida
/// </param>
/// <param name="UnitName">
/// Nombre de la unidad de medida
/// </param>
/// <param name="UnitAbbreviation">
/// Abreviación de la unidad de medida
/// </param>
/// <param name="Tags">
/// Lista de etiquetas del producto
/// </param>
public record ProductResource(
    int Id,
    string Name,
    string Description,
    decimal PurchasePrice,
    decimal SalePrice,
    string InternalNotes,
    int CategoryId,
    string CategoryName,
    int UnitId,
    string UnitName,
    string UnitAbbreviation,
    List<TagResource> Tags
); 