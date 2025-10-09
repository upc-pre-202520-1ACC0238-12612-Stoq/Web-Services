using Lot.AlertStockManagement.Domain.Model.Queries;
using Lot.AlertStockManagement.Domain.Model.Aggregates;
namespace Lot.AlertStockManagement.Domain.Repositories;

/// <summary>
/// Interfaz para el repositorio de lectura de alertas de stock.
/// </summary>
public interface IStockAlertReadRepository
{
    /// <summary>
    /// Obtiene las alertas de stock según el criterio (por ahora, solo por bajo stock).
    /// </summary>
    /// <param name="query">Criterios para la alerta.</param>
    /// <returns>Lista de productos en alerta.</returns>
    Task<List<StockAlertItem>> GetStockAlertsAsync(StockAlertQuery query);
}