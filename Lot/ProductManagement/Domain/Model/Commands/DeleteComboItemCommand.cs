namespace Lot.ProductManagement.Domain.Model.Commands;

/// <summary>
/// Delete Combo Item Command
/// </summary>
/// <param name="Id">
/// El identificador del item de combo a eliminar.
/// </param>
public record DeleteComboItemCommand(int Id);