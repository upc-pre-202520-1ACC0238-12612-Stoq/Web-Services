using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.ProductManagement.Domain.Model.Queries;

namespace Lot.ProductManagement.Domain.Services;

/// <summary>
/// Combo query service interface
/// </summary>
public interface IComboQueryService
{
    /// <summary>
    /// Maneja la consulta de obtener combo por ID.
    /// </summary>
    /// <param name="query">La consulta de obtener combo por ID.</param>
    /// <returns>El combo si se encuentra, de lo contrario null.</returns>
    Task<Combo?> Handle(GetComboByIdQuery query);

    /// <summary>
    /// Maneja la consulta de obtener todos los combos.
    /// </summary>
    /// <param name="query">La consulta de obtener todos los combos.</param>
    /// <returns>Una lista de todos los combos.</returns>
    Task<IEnumerable<Combo>> Handle(GetAllCombosQuery query);
}