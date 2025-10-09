using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.Shared.Domain.Repositories;

namespace Lot.ProductManagement.Domain.Repositories;

/// <summary>
/// Tag repository interface
/// </summary>
public interface ITagRepository : IBaseRepository<Tag>
{
    /// <summary>
    /// Busca una etiqueta por su nombre.
    /// </summary>
    /// <param name="name">El nombre de la etiqueta a buscar.</param>
    /// <returns>La etiqueta si se encuentra, de lo contrario null.</returns>
    Task<Tag?> FindTagByNameAsync(string name);

    /// <summary>
    /// Busca todas las etiquetas.
    /// </summary>
    /// <returns>Una lista de todas las etiquetas.</returns>
    Task<IEnumerable<Tag>> FindAllTagsAsync();

    /// <summary>
    /// Busca etiquetas por sus identificadores.
    /// </summary>
    /// <param name="tagIds">Los identificadores de las etiquetas.</param>
    /// <returns>Una lista de etiquetas con los identificadores especificados.</returns>
    Task<IEnumerable<Tag>> FindTagsByIdsAsync(List<int> tagIds);
} 