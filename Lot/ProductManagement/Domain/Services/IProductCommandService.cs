using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.ProductManagement.Domain.Model.Commands;

namespace Lot.ProductManagement.Domain.Services;

/// <summary>
/// Product command service interface
/// </summary>
public interface IProductCommandService
{
    /// <summary>
    /// Maneja el comando de crear producto.
    /// </summary>
    /// <param name="command">El comando de crear producto.</param>
    /// <returns>El producto creado.</returns>
    Task<Product?> Handle(CreateProductCommand command);

    /// <summary>
    /// Maneja el comando de actualizar producto.
    /// </summary>
    /// <param name="command">El comando de actualizar producto.</param>
    /// <returns>El producto actualizado.</returns>
    Task<Product?> Handle(UpdateProductCommand command);

    /// <summary>
    /// Maneja el comando de eliminar producto.
    /// </summary>
    /// <param name="command">El comando de eliminar producto.</param>
    /// <returns>True si el producto fue eliminado, false de lo contrario.</returns>
    Task<bool> Handle(DeleteProductCommand command);
} 