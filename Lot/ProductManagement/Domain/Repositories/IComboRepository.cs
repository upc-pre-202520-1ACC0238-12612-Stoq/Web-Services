using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.Shared.Domain.Repositories;

namespace Lot.ProductManagement.Domain.Repositories;

/// <summary>
/// Combo repository interface
/// </summary>
public interface IComboRepository : IBaseRepository<Combo>
{
    /// <summary>
    /// Busca un combo por su nombre.
    /// </summary>
    /// <param name="name">El nombre del combo a buscar.</param>
    /// <returns>El combo si se encuentra, de lo contrario null.</returns>
    Task<Combo?> FindComboByNameAsync(string name);

    /// <summary>
    /// Busca todos los combos.
    /// </summary>
    /// <returns>Una lista de todos los combos.</returns>
    Task<IEnumerable<Combo>> FindAllCombosAsync();

    /// <summary>
    /// Busca un combo con sus relaciones incluidas.
    /// </summary>
    /// <param name="id">El identificador del combo.</param>
    /// <returns>El combo con sus relaciones si se encuentra, de lo contrario null.</returns>
    Task<Combo?> FindComboWithRelationsAsync(int id);
}