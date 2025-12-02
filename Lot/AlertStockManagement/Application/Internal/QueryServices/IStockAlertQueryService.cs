namespace Lot.AlertStockManagement.Application.Internal.QueryServices;

using Lot.AlertStockManagement.Domain.Model.Aggregates;
using Lot.AlertStockManagement.Domain.Model.Queries;

public interface IStockAlertQueryService
{
    Task<List<StockAlertItem>> GetAlertsAsync(StockAlertQuery query);
}