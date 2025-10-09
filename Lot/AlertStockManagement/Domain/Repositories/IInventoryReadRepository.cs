using Lot.AlertStockManagement.Domain.Model.Aggregates;
using Lot.AlertStockManagement.Domain.Model.Queries;

namespace Lot.AlertStockManagement.Domain.Repositories;

public interface IInventoryReadRepository
{
    Task<List<StockAlertItem>> GetStockAlertsAsync(StockAlertQuery query);
}

