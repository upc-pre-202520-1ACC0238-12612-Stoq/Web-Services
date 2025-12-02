using Lot.AlertStockManagement.Application.Internal.QueryServices;
using Lot.AlertStockManagement.Domain.Model.Queries;
using Lot.AlertStockManagement.Interfaces.REST.Transform;
using Lot.AlertStockManagement.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;
using Lot.IAM.Infrastructure.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.Extensions.Logging;

namespace Lot.AlertStockManagement.Interfaces.REST.Controllers;

[ApiController]
[Route("api/alerts")]
[Produces("application/json")]
[SwaggerTag("Gestión de alertas de stock y nivel de inventario")]
[AuthorizeRoles("Administrator", "Employee")]
public class StockAlertController : ControllerBase
{
    private readonly IStockAlertQueryService _service;
    private readonly ILogger<StockAlertController> _logger;

    public StockAlertController(IStockAlertQueryService service, ILogger<StockAlertController> logger)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Obtiene todas las alertas de stock activas
    /// </summary>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Obtener alertas de stock",
        Description = "Retorna todos los productos con stock bajo o crítico",
        OperationId = "GetStockAlerts")]
    [SwaggerResponse(200, "Alertas obtenidas exitosamente", typeof(IEnumerable<StockAlertResource>))]
    public async Task<IActionResult> GetAlerts()
    {
        try
        {
            _logger.LogInformation("Iniciando consulta de alertas de stock");

            var query = CreateDefaultQuery();
            var result = await _service.GetAlertsAsync(query);
            var resources = result.Select(StockAlertTransform.ToResource).ToList();

            var response = CreateAlertsResponse(resources, "Alertas de stock obtenidas exitosamente");
            _logger.LogInformation("Consulta completada. {Count} alertas encontradas", resources.Count);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener alertas de stock");
            return StatusCode(500, new { Message = "Error interno del servidor al obtener alertas" });
        }
    }

    /// <summary>
    /// Obtiene alertas filtradas por categoría
    /// </summary>
    [HttpGet("by-category")]
    [SwaggerOperation(
        Summary = "Obtener alertas por categoría",
        Description = "Filtra las alertas por categoría específica",
        OperationId = "GetAlertsByCategory")]
    public async Task<IActionResult> GetAlertsByCategory(
        [FromQuery] int? categoryId,
        [FromQuery] string? categoryName)
    {
        try
        {
            _logger.LogInformation("Consultando alertas por categoría: CategoryId={CategoryId}, CategoryName={CategoryName}",
                categoryId, categoryName);

            var query = CreateDefaultQuery();
            query.CategoryId = categoryId;
            query.CategoryName = categoryName?.Trim();

            var result = await _service.GetAlertsAsync(query);
            var resources = result.Select(StockAlertTransform.ToResource).ToList();

            var categoryDisplay = string.IsNullOrWhiteSpace(categoryName)
                ? $"ID {categoryId}"
                : categoryName;

            var response = new
            {
                Message = $"Alertas para categoría: {categoryDisplay}",
                TotalAlerts = resources.Count,
                Alerts = resources
            };

            _logger.LogInformation("Consulta por categoría completada. {Count} alertas encontradas", resources.Count);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener alertas por categoría");
            return StatusCode(500, new { Message = "Error interno del servidor al obtener alertas por categoría" });
        }
    }

    /// <summary>
    /// Obtiene resumen de alertas con estadísticas
    /// </summary>
    [HttpGet("summary")]
    [SwaggerOperation(
        Summary = "Obtener resumen de alertas",
        Description = "Retorna estadísticas resumidas de las alertas de stock",
        OperationId = "GetAlertsSummary")]
    public async Task<IActionResult> GetAlertsSummary()
    {
        try
        {
            _logger.LogInformation("Generando resumen de alertas de stock");

            var query = CreateDefaultQuery();
            var result = await _service.GetAlertsAsync(query);
            var alerts = result.Select(StockAlertTransform.ToResource).ToList();

            var stats = CalculateAlertStatistics(alerts);
            var summary = new
            {
                TotalProducts = alerts.Count,
                CriticalAlerts = stats.Critical,
                HighAlerts = stats.High,
                MediumAlerts = stats.Medium,
                TotalDeficitValue = alerts.Sum(a => a.StockDeficit * a.Price),
                CategoriesWithAlerts = alerts.Select(a => a.CategoryName).Distinct().Count(),
                LastUpdated = DateTime.Now,
                MostCriticalProducts = GetMostCriticalProducts(alerts)
            };

            _logger.LogInformation("Resumen generado. Total: {Total}, Críticos: {Critical}",
                summary.TotalProducts, summary.CriticalAlerts);

            return Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al generar resumen de alertas");
            return StatusCode(500, new { Message = "Error interno del servidor al generar resumen de alertas" });
        }
    }

    /// <summary>
    /// Obtiene los productos más críticos de manera eficiente
    /// </summary>
    private static object[] GetMostCriticalProducts(List<StockAlertResource> alerts)
    {
        return alerts
            .Where(a => a.AlertLevel.Contains("Crítico", StringComparison.OrdinalIgnoreCase) ||
                       a.AlertLevel.Contains("Alto", StringComparison.OrdinalIgnoreCase))
            .OrderBy(a => a.Quantity)
            .ThenBy(a => a.ProductName)
            .Take(5)
            .Select(a => new { a.ProductName, a.Quantity, a.AlertLevel })
            .ToArray();
    }

    #region Métodos Privados de Ayuda

    /// <summary>
    /// Crea una query por defecto para alertas de stock
    /// </summary>
    private static StockAlertQuery CreateDefaultQuery()
    {
        return new StockAlertQuery();
    }

    /// <summary>
    /// Crea una respuesta estandarizada para alertas
    /// </summary>
    private static object CreateAlertsResponse(List<StockAlertResource> resources, string message)
    {
        var stats = CalculateAlertStatistics(resources);
        return new
        {
            Message = message,
            TotalAlerts = resources.Count,
            CriticalAlerts = stats.Critical,
            HighAlerts = stats.High,
            MediumAlerts = stats.Medium,
            Alerts = resources
        };
    }

    /// <summary>
    /// Calcula estadísticas de alertas de manera eficiente
    /// </summary>
    private static (int Critical, int High, int Medium) CalculateAlertStatistics(List<StockAlertResource> resources)
    {
        int critical = 0, high = 0, medium = 0;

        foreach (var alert in resources)
        {
            if (alert.AlertLevel.Contains("Crítico", StringComparison.OrdinalIgnoreCase))
                critical++;
            else if (alert.AlertLevel.Contains("Alto", StringComparison.OrdinalIgnoreCase))
                high++;
            else if (alert.AlertLevel.Contains("Medio", StringComparison.OrdinalIgnoreCase))
                medium++;
        }

        return (critical, high, medium);
    }

    #endregion
}