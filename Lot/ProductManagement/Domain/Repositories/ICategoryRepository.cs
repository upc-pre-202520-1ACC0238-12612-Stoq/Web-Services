using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.Shared.Domain.Repositories;

namespace Lot.ProductManagement.Domain.Repositories;

/// <summary>
/// Category repository interface
/// </summary>
public interface ICategoryRepository : IBaseRepository<Category>
{
    /// <summary>
    /// Busca una categoría por su nombre.
    /// </summary>
    /// <param name="name">El nombre de la categoría a buscar.</param>
    /// <returns>La categoría si se encuentra, de lo contrario null.</returns>
    Task<Category?> FindCategoryByNameAsync(string name);

    /// <summary>
    /// Busca todas las categorías.
    /// </summary>
    /// <returns>Una lista de todas las categorías.</returns>
    Task<IEnumerable<Category>> FindAllCategoriesAsync();
} 