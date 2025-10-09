using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.ProductManagement.Domain.Model.Commands;

namespace Lot.ProductManagement.Domain.Services;

/// <summary>
/// Combo command service interface
/// </summary>
public interface IComboCommandService
{
    /// <summary>
    /// Maneja el comando de crear combo.
    /// </summary>
    /// <param name="command">El comando de crear combo.</param>
    /// <returns>El combo creado.</returns>
    Task<Combo?> Handle(CreateComboCommand command);

    /// <summary>
    /// Maneja el comando de actualizar combo.
    /// </summary>
    /// <param name="command">El comando de actualizar combo.</param>
    /// <returns>El combo actualizado.</returns>
    Task<Combo?> Handle(UpdateComboCommand command);

    /// <summary>
    /// Maneja el comando de eliminar combo.
    /// </summary>
    /// <param name="command">El comando de eliminar combo.</param>
    /// <returns>True si el combo fue eliminado, false de lo contrario.</returns>
    Task<bool> Handle(DeleteComboCommand command);
}
