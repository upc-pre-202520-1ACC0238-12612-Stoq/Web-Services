namespace Lot.ProductManagement.Domain.Model.Commands;

/// <summary>
/// Create Combo Item Command
/// </summary>
/// <param name="ComboId">
/// El identificador del combo.
/// </param>
/// <param name="ProductId">
/// El identificador del producto.
/// </param>
/// <param name="Quantity">
/// La cantidad del producto en el combo.
/// </param>
public record CreateComboItemCommand(
    int ComboId,
    int ProductId,
    int Quantity
);