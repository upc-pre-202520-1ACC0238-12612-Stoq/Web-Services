using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using Lot.Inventaries.Domain.Model.Commands;
using Lot.Inventaries.Domain.Model.Queries;
using Lot.Inventaries.Domain.Services;
using Lot.Inventaries.Interfaces.REST.Resources;
using Lot.Inventaries.Interfaces.REST.Transform;
using Lot.IAM.Infrastructure.Authorization;

namespace Lot.Inventaries.Interfaces.REST;

[ApiController]
[Route("api/v1/branches")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Operaciones de gestión de sucursales (branches).")]
[AuthorizeRoles("Administrator", "Employee")]
public class BranchController : ControllerBase
{
    private readonly IBranchCommandService _commandService;
    private readonly IBranchQueryService _queryService;

    public BranchController(
        IBranchCommandService commandService,
        IBranchQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Obtener todas las sucursales",
        Description = "Lista todas las sucursales registradas, opcionalmente filtradas por tipo.",
        OperationId = "GetAllBranches")]
    [SwaggerResponse(StatusCodes.Status200OK, "Lista de sucursales obtenida.", typeof(IEnumerable<BranchResource>))]
    public async Task<IActionResult> GetAllBranches([FromQuery] string? type)
    {
        var query = new GetBranchQuery(type: type);
        var branches = await _queryService.Handle(query);
        var resources = branches.Select(BranchResourceAssembler.ToResource);
        return Ok(resources);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Obtener sucursal por ID",
        Description = "Obtiene los detalles de una sucursal específica.",
        OperationId = "GetBranchById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Sucursal encontrada.", typeof(BranchResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Sucursal no encontrada.")]
    public async Task<IActionResult> GetBranchById(int id)
    {
        var branch = await _queryService.GetByIdAsync(id);
        if (branch == null) return NotFound();
        return Ok(BranchResourceAssembler.ToResource(branch));
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Crear nueva sucursal",
        Description = "Crea una nueva sucursal con la información proporcionada.",
        OperationId = "CreateBranch")]
    [SwaggerResponse(StatusCodes.Status201Created, "Sucursal creada correctamente.", typeof(BranchResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos inválidos.")]
    public async Task<IActionResult> CreateBranch([FromBody] CreateBranchResource resource)
    {
        var command = BranchResourceAssembler.ToCommandFromResource(resource);
        var result = await _commandService.Handle(command);
        
        if (result == null)
            return BadRequest("No se pudo crear la sucursal.");
        
        var response = BranchResourceAssembler.ToResource(result);
        return CreatedAtAction(nameof(GetBranchById), new { id = result.Id }, response);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Actualizar sucursal",
        Description = "Actualiza la información de una sucursal existente.",
        OperationId = "UpdateBranch")]
    [SwaggerResponse(StatusCodes.Status200OK, "Sucursal actualizada correctamente.", typeof(BranchResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Sucursal no encontrada.")]
    public async Task<IActionResult> UpdateBranch(int id, [FromBody] CreateBranchResource resource)
    {
        var command = new UpdateBranchCommand(
            id,
            resource.Name,
            resource.Type,
            resource.Address,
            resource.Latitude,
            resource.Longitude
        );
        
        var result = await _commandService.Handle(command);
        if (result == null) return NotFound();
        
        return Ok(BranchResourceAssembler.ToResource(result));
    }

    [HttpPatch("{id}/stock")]
    [SwaggerOperation(
        Summary = "Actualizar stock de sucursal",
        Description = "Actualiza el stock total de una sucursal.",
        OperationId = "UpdateBranchStock")]
    [SwaggerResponse(StatusCodes.Status200OK, "Stock actualizado correctamente.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Sucursal no encontrada.")]
    public async Task<IActionResult> UpdateBranchStock(int id, [FromBody] int stockTotal)
    {
        var updated = await _commandService.UpdateStockAsync(id, stockTotal);
        if (!updated) return NotFound();
        return Ok(new { message = "Stock actualizado correctamente", stockTotal });
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Eliminar sucursal",
        Description = "Elimina una sucursal del sistema.",
        OperationId = "DeleteBranch")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Sucursal eliminada.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Sucursal no encontrada.")]
    public async Task<IActionResult> DeleteBranch(int id)
    {
        var deleted = await _commandService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}

