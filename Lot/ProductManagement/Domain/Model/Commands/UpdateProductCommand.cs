namespace Lot.ProductManagement.Domain.Model.Commands;

/// <summary>
/// Update Product Command
/// </summary>
/// <param name="Id">
/// El identificador del producto a actualizar.
/// </param>
/// <param name="Name">
/// El nuevo nombre del producto.
/// </param>
/// <param name="Description">
/// La nueva descripción del producto.
/// </param>
/// <param name="PurchasePrice">
/// El nuevo precio de compra del producto.
/// </param>
/// <param name="SalePrice">
/// El nuevo precio de venta del producto.
/// </param>
/// <param name="InternalNotes">
/// Las nuevas notas internas del producto.
/// </param>
/// <param name="CategoryId">
/// El nuevo identificador de la categoría del producto.
/// </param>
/// <param name="UnitId">
/// El nuevo identificador de la unidad de medida del producto.
/// </param>
/// <param name="TagIds">
/// Los nuevos identificadores de las etiquetas del producto.
/// </param>
public record UpdateProductCommand(
    int Id,
    string Name,
    string Description,
    decimal PurchasePrice,
    decimal SalePrice,
    string InternalNotes,
    int CategoryId,
    int UnitId,
    List<int> TagIds
); 