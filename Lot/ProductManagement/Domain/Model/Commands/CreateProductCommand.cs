namespace Lot.ProductManagement.Domain.Model.Commands;

/// <summary>
/// Create Product Command
/// </summary>
/// <param name="Name">
/// El nombre del producto.
/// </param>
/// <param name="Description">
/// La descripción del producto.
/// </param>
/// <param name="PurchasePrice">
/// El precio de compra del producto.
/// </param>
/// <param name="SalePrice">
/// El precio de venta del producto.
/// </param>
/// <param name="InternalNotes">
/// Las notas internas del producto.
/// </param>
/// <param name="CategoryId">
/// El identificador de la categoría del producto.
/// </param>
/// <param name="UnitId">
/// El identificador de la unidad de medida del producto.
/// </param>
/// <param name="TagIds">
/// Los identificadores de las etiquetas del producto.
/// </param>
public record CreateProductCommand(
    string Name,
    string Description,
    decimal PurchasePrice,
    decimal SalePrice,
    string InternalNotes,
    int CategoryId,
    int UnitId,
    List<int> TagIds
); 