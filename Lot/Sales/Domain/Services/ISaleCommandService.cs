using Lot.Sales.Domain.Model.Commands;
using Lot.Sales.Domain.Model.Aggregates;

namespace Lot.Sales.Domain.Services;

/// <summary>
/// Interfaz del servicio de comandos de ventas con integración automática de inventario.
/// Maneja la creación de ventas y la reducción automática de stock.
/// </summary>
public interface ISaleCommandService
{
    /// <summary>
    /// Crea una nueva venta con reducción automática de inventario
    /// </summary>
    /// <param name="command">Command con datos de la venta</param>
    /// <returns>Venta creada o null si hubo error</returns>
    Task<Sale?> Handle(CreateSaleCommand command);

    /// <summary>
    /// Elimina una venta (restaura stock si se configura)
    /// </summary>
    Task<bool> DeleteAsync(int id);
}