using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.ProductManagement.Domain.Model.Queries;

namespace Lot.ProductManagement.Domain.Services;

/// <summary>
/// Product query service interface
/// </summary>
public interface IProductQueryService
{
    /// <summary>
    /// Maneja la consulta de obtener producto por ID.
    /// </summary>
    /// <param name="query">La consulta de obtener producto por ID.</param>
    /// <returns>El producto si se encuentra, de lo contrario null.</returns>
    Task<Product?> Handle(GetProductByIdQuery query);

    /// <summary>
    /// Maneja la consulta de obtener todos los productos.
    /// </summary>
    /// <param name="query">La consulta de obtener todos los productos.</param>
    /// <returns>Una lista de todos los productos.</returns>
    Task<IEnumerable<Product>> Handle(GetAllProductsQuery query);

    /// <summary>
    /// Maneja la consulta de obtener productos por categoría.
    /// </summary>
    /// <param name="query">La consulta de obtener productos por categoría.</param>
    /// <returns>Una lista de productos de la categoría especificada.</returns>
    Task<IEnumerable<Product>> Handle(GetProductsByCategoryQuery query);

    /// <summary>
    /// Maneja la consulta de obtener productos por etiqueta.
    /// </summary>
    /// <param name="query">La consulta de obtener productos por etiqueta.</param>
    /// <returns>Una lista de productos con la etiqueta especificada.</returns>
    Task<IEnumerable<Product>> Handle(GetProductsByTagQuery query);

    /// <summary>
    /// Maneja la consulta de obtener productos por rango de precios.
    /// </summary>
    /// <param name="query">La consulta de obtener productos por rango de precios.</param>
    /// <returns>Una lista de productos dentro del rango de precios especificado.</returns>
    Task<IEnumerable<Product>> Handle(GetProductsByPriceRangeQuery query);
} 