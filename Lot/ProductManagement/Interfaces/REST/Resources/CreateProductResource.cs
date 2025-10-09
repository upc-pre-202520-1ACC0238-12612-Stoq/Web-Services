namespace Lot.ProductManagement.Interfaces.REST.Resources;

/// <summary>
/// Recurso para crear un producto
/// </summary>
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
/// <param name="UnitId">
/// Identificador de la unidad de medida
/// </param>
/// <param name="TagIds">
/// Lista de identificadores de etiquetas
/// </param>
public record CreateProductResource(
    string Name,
    string Description,
    decimal PurchasePrice,
    decimal SalePrice,
    string InternalNotes,
    int CategoryId,
    int UnitId,
    List<int> TagIds
); 