using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.Shared.Domain.Repositories;

namespace Lot.ProductManagement.Domain.Repositories;

/// <summary>
/// Product repository interface
/// </summary>
public interface IProductRepository : IBaseRepository<Product>
{
    /// <summary>
    /// Busca un producto por su nombre.
    /// </summary>
    /// <param name="name">El nombre del producto a buscar.</param>
    /// <returns>El producto si se encuentra, de lo contrario null.</returns>
    Task<Product?> FindProductByNameAsync(string name);

    /// <summary>
    /// Busca todos los productos.
    /// </summary>
    /// <returns>Una lista de todos los productos.</returns>
    Task<IEnumerable<Product>> FindAllProductsAsync();

    /// <summary>
    /// Busca productos por categoría.
    /// </summary>
    /// <param name="categoryId">El identificador de la categoría.</param>
    /// <returns>Una lista de productos de la categoría especificada.</returns>
    Task<IEnumerable<Product>> FindProductsByCategoryAsync(int categoryId);

    /// <summary>
    /// Busca productos por etiqueta.
    /// </summary>
    /// <param name="tagId">El identificador de la etiqueta.</param>
    /// <returns>Una lista de productos con la etiqueta especificada.</returns>
    Task<IEnumerable<Product>> FindProductsByTagAsync(int tagId);

    /// <summary>
    /// Busca productos por rango de precios.
    /// </summary>
    /// <param name="minPrice">El precio mínimo.</param>
    /// <param name="maxPrice">El precio máximo.</param>
    /// <returns>Una lista de productos dentro del rango de precios especificado.</returns>
    Task<IEnumerable<Product>> FindProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);

    /// <summary>
    /// Busca un producto con sus relaciones incluidas.
    /// </summary>
    /// <param name="id">El identificador del producto.</param>
    /// <returns>El producto con sus relaciones si se encuentra, de lo contrario null.</returns>
    Task<Product?> FindProductWithRelationsAsync(int id);
} 