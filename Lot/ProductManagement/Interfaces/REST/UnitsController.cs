using System.Net.Mime;
using Lot.ProductManagement.Domain.Services;
using Lot.ProductManagement.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Lot.IAM.Infrastructure.Authorization;

namespace Lot.ProductManagement.Interfaces.REST;

[ApiController]
[Route("api/v1/units")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Puntos de acceso para la gestión de unidades de medida.")]
[AuthorizeRoles("Employee", "Administrator")]
public class UnitsController : ControllerBase
{
    private readonly IUnitQueryService _unitQueryService;

    public UnitsController(IUnitQueryService unitQueryService)
    {
        _unitQueryService = unitQueryService;
    }

    [HttpGet]
    [SwaggerOperation("Obtener todas las unidades de medida", OperationId = "GetAllUnits")]
    [SwaggerResponse(200, "Unidades encontradas.", typeof(IEnumerable<UnitResource>))]
    public async Task<IActionResult> GetAllUnits()
    {
        // Log del rol recibido
        var user = HttpContext.User;
        var userRole = user.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? user.FindFirst("role")?.Value;
        Console.WriteLine($"[LOG] Rol recibido en UnitsController: {userRole}");
        var units = await _unitQueryService.GetAllAsync();
        var resources = units.Select(u => new UnitResource(u.Id, u.Name, u.Abbreviation));
        return Ok(resources);
    }
} 