using System.Net.Mime;
using Lot.ProductManagement.Domain.Model.Commands;
using Lot.ProductManagement.Domain.Model.Queries;
using Lot.ProductManagement.Domain.Services;
using Lot.ProductManagement.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Lot.IAM.Infrastructure.Authorization;

namespace Lot.ProductManagement.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Puntos de acceso disponibles para la gestión de combos (kits).")]
[AuthorizeRoles("Administrator", "Employee")]
public class CombosController(
    IComboCommandService comboCommandService,
    IComboQueryService comboQueryService)
    : ControllerBase
{
    [HttpGet("{comboId:int}")]
    [SwaggerOperation("Obtener Combo por Id", "Obtiene un combo por su identificador único.", OperationId = "GetComboById")]
    [SwaggerResponse(200, "El combo fue encontrado y retornado.", typeof(ComboResource))]
    [SwaggerResponse(404, "El combo no fue encontrado.")]
    public async Task<IActionResult> GetComboById(int comboId)
    {
        var combo = await comboQueryService.Handle(new GetComboByIdQuery(comboId));
        if (combo is null) return NotFound();
        
        var comboItems = combo.ComboItems.Select(ci => new ComboItemResource(
            ci.Id,
            ci.ProductId,
            ci.Product?.Name ?? string.Empty,
            ci.Product?.Description ?? string.Empty,
            ci.Product?.SalePrice ?? 0,
            ci.Quantity
        )).ToList();
        
        var resource = new ComboResource(
            combo.Id,
            combo.Name,
            comboItems
        );
        
        return Ok(resource);
    }

    [HttpPost]
    [SwaggerOperation("Crear Combo", "Crea un nuevo combo (kit).", OperationId = "CreateCombo")]
    [SwaggerResponse(201, "El combo fue creado.", typeof(ComboResource))]
    [SwaggerResponse(400, "El combo no fue creado.")]
    public async Task<IActionResult> CreateCombo(CreateComboResource resource)
    {
        try
        {
            var items = resource.Items.Select(i => new ComboItemCommand(i.ProductId, i.Quantity)).ToList();
            var command = new CreateComboCommand(resource.Name, items);
            
            var combo = await comboCommandService.Handle(command);
            if (combo is null) return BadRequest("Error al crear el combo");
            
            var comboItems = combo.ComboItems.Select(ci => new ComboItemResource(
                ci.Id,
                ci.ProductId,
                ci.Product?.Name ?? string.Empty,
                ci.Product?.Description ?? string.Empty,
                ci.Product?.SalePrice ?? 0,
                ci.Quantity
            )).ToList();
            
            var comboResource = new ComboResource(
                combo.Id,
                combo.Name,
                comboItems
            );
            
            return CreatedAtAction(nameof(GetComboById), new { comboId = combo.Id }, comboResource);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }

    [HttpGet]
    [SwaggerOperation("Obtener Todos los Combos", "Obtiene todos los combos (kits).", OperationId = "GetAllCombos")]
    [SwaggerResponse(200, "Los combos fueron encontrados y retornados.", typeof(IEnumerable<ComboResource>))]
    public async Task<IActionResult> GetAllCombos()
    {
        var combos = await comboQueryService.Handle(new GetAllCombosQuery());
        
        var resources = combos.Select(combo => {
            var comboItems = combo.ComboItems.Select(ci => new ComboItemResource(
                ci.Id,
                ci.ProductId,
                ci.Product?.Name ?? string.Empty,
                ci.Product?.Description ?? string.Empty,
                ci.Product?.SalePrice ?? 0,
                ci.Quantity
            )).ToList();
            
            return new ComboResource(
                combo.Id,
                combo.Name,
                comboItems
            );
        });
        
        return Ok(resources);
    }
}