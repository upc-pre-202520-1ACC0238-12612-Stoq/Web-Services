using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.Shared.Domain.Repositories;

namespace Lot.ProductManagement.Domain.Repositories;

/// <summary>
/// Unit repository interface
/// </summary>
public interface IUnitRepository : IBaseRepository<Unit>
{
    /// <summary>
    /// Busca una unidad por su nombre.
    /// </summary>
    /// <param name="name">El nombre de la unidad a buscar.</param>
    /// <returns>La unidad si se encuentra, de lo contrario null.</returns>
    Task<Unit?> FindUnitByNameAsync(string name);

    /// <summary>
    /// Busca una unidad por su abreviación.
    /// </summary>
    /// <param name="abbreviation">La abreviación de la unidad a buscar.</param>
    /// <returns>La unidad si se encuentra, de lo contrario null.</returns>
    Task<Unit?> FindUnitByAbbreviationAsync(string abbreviation);

    /// <summary>
    /// Busca todas las unidades.
    /// </summary>
    /// <returns>Una lista de todas las unidades.</returns>
    Task<IEnumerable<Unit>> FindAllUnitsAsync();
} 