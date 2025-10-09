namespace Lot.ProductManagement.Domain.Model.Commands;

/// <summary>
/// Delete Product Command
/// </summary>
/// <param name="Id">
/// El identificador del producto a eliminar.
/// </param>
public record DeleteProductCommand(int Id); 