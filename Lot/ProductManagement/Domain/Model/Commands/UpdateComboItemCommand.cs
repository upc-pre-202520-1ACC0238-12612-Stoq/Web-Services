namespace Lot.ProductManagement.Domain.Model.Commands;

/// <summary>
/// Update Combo Item Command
/// </summary>
/// <param name="Id">
/// El identificador del item de combo a actualizar.
/// </param>
/// <param name="Quantity">
/// La nueva cantidad del producto en el combo.
/// </param>
public record UpdateComboItemCommand(
    int Id,
    int Quantity
);