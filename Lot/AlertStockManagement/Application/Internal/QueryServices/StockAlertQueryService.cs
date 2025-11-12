using Lot.AlertStockManagement.Domain.Model.Aggregates;
using Lot.AlertStockManagement.Domain.Repositories;
using Lot.AlertStockManagement.Domain.Model.Queries;
using Microsoft.Extensions.Logging;

namespace Lot.AlertStockManagement.Application.Internal.QueryServices;

public class StockAlertQueryService : IStockAlertQueryService
{
    private readonly IInventoryReadRepository _repository;
    private readonly ILogger<StockAlertQueryService> _logger;

    public StockAlertQueryService(IInventoryReadRepository repository, ILogger<StockAlertQueryService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<StockAlertItem>> GetAlertsAsync(StockAlertQuery query)
    {
        try
        {
            _logger.LogDebug("Ejecutando servicio de consultas de alertas");

            if (query == null)
            {
                _logger.LogError("Query parameter is null");
                throw new ArgumentNullException(nameof(query));
            }

            var result = await _repository.GetStockAlertsAsync(query);
            _logger.LogInformation("Consulta completada. {Count} alertas encontradas", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en StockAlertQueryService.GetAlertsAsync");
            throw;
        }
    }
}