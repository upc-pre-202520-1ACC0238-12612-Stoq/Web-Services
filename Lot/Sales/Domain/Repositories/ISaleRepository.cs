using Lot.Sales.Domain.Model.Aggregates;

namespace Lot.Sales.Domain.Repositories;

/// <summary>
/// Interfaz del repositorio de ventas siguiendo el patrón Repository.
/// Gestiona la persistencia de agregados de venta.
/// </summary>
public interface ISaleRepository
{
    Task<IEnumerable<Sale>> ListAsync();
    Task<Sale?> FindByIdAsync(int id);
    Task AddAsync(Sale entity);
    Task UpdateAsync(Sale entity);
    Task DeleteAsync(Sale entity);
    Task DeleteAsync(int id);

    /// <summary>
    /// Obtiene todas las ventas de un producto específico
    /// </summary>
    Task<IEnumerable<Sale>> GetByProductIdAsync(int productId);

    /// <summary>
    /// Obtiene ventas por rango de fechas
    /// </summary>
    Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Obtiene ventas por nombre de cliente
    /// </summary>
    Task<IEnumerable<Sale>> GetByCustomerNameAsync(string customerName);

    /// <summary>
    /// Obtiene las ventas más recientes
    /// </summary>
    Task<IEnumerable<Sale>> GetRecentSalesAsync(int limit = 10);

    /// <summary>
    /// Obtiene el total de ventas por día
    /// </summary>
    Task<decimal> GetTotalSalesByDateAsync(DateTime date);

    /// <summary>
    /// Obtiene el total de vendido por producto
    /// </summary>
    Task<int> GetTotalQuantitySoldByProductAsync(int productId);
}