using Lot.AlertStockManagement.Application.Internal.QueryServices;
using Lot.AlertStockManagement.Domain.Model.Queries;
using Lot.AlertStockManagement.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Lot.IAM.Infrastructure.Authorization;

namespace Lot.AlertStockManagement.Interfaces.REST.Controllers;

[ApiController]
[Route("api/alerts")]
[AuthorizeRoles("Administrator", "Employee")]
public class StockAlertController : ControllerBase
{
    private readonly StockAlertQueryService _service;

    public StockAlertController(StockAlertQueryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAlerts()
    {
        // Creamos la query que, por ahora, solo filtra por bajo stock
        var query = new StockAlertQuery
        {
            IncludeLowStock = true // en el futuro puedes agregar más propiedades como IncludeExpiry
        };

        // Llamamos al servicio de aplicación
        var result = await _service.GetAlertsAsync(query);

        // Transformamos los resultados a recursos para el response HTTP
        var resources = result.Select(StockAlertTransform.ToResource).ToList();

        // Retornamos respuesta 200 OK con la lista de alertas
        return Ok(resources);
    }
}