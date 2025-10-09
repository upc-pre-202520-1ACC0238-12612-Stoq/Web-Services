using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.Shared.Domain.Repositories;

namespace Lot.ProductManagement.Domain.Repositories;

/// <summary>
/// Combo Item repository interface
/// </summary>
public interface IComboItemRepository : IBaseRepository<ComboItem>
{
    /// <summary>
    /// Busca items de combo por el id del combo.
    /// </summary>
    /// <param name="comboId">El id del combo.</param>
    /// <returns>Los items del combo si se encuentran.</returns>
    Task<IEnumerable<ComboItem>> FindByComboIdAsync(int comboId);
}