using Lot.AlertStockManagement.Domain.Model.Aggregates;
using Lot.AlertStockManagement.Domain.Repositories;
using Lot.AlertStockManagement.Domain.Model.Queries;
namespace Lot.AlertStockManagement.Application.Internal.QueryServices;

public class StockAlertQueryService
{
    private readonly IInventoryReadRepository _repository;

    public StockAlertQueryService(IInventoryReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<StockAlertItem>> GetAlertsAsync(StockAlertQuery query)
    {
        return await _repository.GetStockAlertsAsync(query);
    }
}