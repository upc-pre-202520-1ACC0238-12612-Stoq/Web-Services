using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using Lot.Inventaries.Domain.Model.Aggregates;
using Lot.Inventaries.Domain.Model.Commands;
using Lot.Inventaries.Domain.Model.Queries;
using Lot.Inventaries.Domain.Services;
using Lot.Inventaries.Interfaces.REST.Resources;
using Lot.Inventaries.Interfaces.REST.Transform;
using Lot.IAM.Infrastructure.Authorization; 


namespace Lot.Inventaries.Interfaces.REST;

[ApiController]
[Route("api/v1/inventory")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Operaciones del inventario general, por producto y por lote.")]
[AuthorizeRoles("Administrator", "Employee")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryByProductCommandService _productCommandService;
    private readonly IInventoryByProductQueryService _productQueryService;
    private readonly IInventoryByBatchCommandService _batchCommandService;
    private readonly IInventoryByBatchQueryService _batchQueryService;

    public InventoryController(
        IInventoryByProductCommandService productCommandService,
        IInventoryByProductQueryService productQueryService,
        IInventoryByBatchCommandService batchCommandService,
        IInventoryByBatchQueryService batchQueryService)
    {
        _productCommandService = productCommandService;
        _productQueryService = productQueryService;
        _batchCommandService = batchCommandService;
        _batchQueryService = batchQueryService;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Obtener todos los registros de inventario",
        Description = "Resumen general del inventario, combinando productos y lotes.",
        OperationId = "GetAllInventory")]
    [SwaggerResponse(StatusCodes.Status200OK, "Lista general obtenida.", typeof(InventoryGeneralResource))]
    public async Task<IActionResult> GetAllInventory()
    {
        var products = await _productQueryService.GetAllAsync();
        var batches = await _batchQueryService.Handle(new GetInventoryByBatchQuery());

        var response = new InventoryGeneralResource
        {
            Productos = products.Select(p => InventoryByProductResourceAssembler.ToResourceFromEntity(p)).ToList(),
            Lotes = batches.Select(b => InventoryByBatchResourceAssembler.ToResource(b)).ToList(),
        };

        return Ok(response);
    }


    // PRODUCTOS

    [HttpPost("by-product")]
    [SwaggerOperation("Crear Inventario por Producto", OperationId = "CreateInventoryByProduct")]
    [SwaggerResponse(StatusCodes.Status201Created, "Inventario por producto creado correctamente.", typeof(InventoryByProductResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos inválidos proporcionados.")]
    public async Task<IActionResult> CreateByProduct([FromBody] CreateInventoryByProductResource resource)
    {
        try
        {
            // Transform Resource → Command siguiendo el patrón DDD
            var command = CreateInventoryByProductCommandAssembler.ToCommandFromResource(resource);

            // Manejar el Command a través del Application Service
            var result = await _productCommandService.Handle(command);

            if (result == null)
                return BadRequest("No se pudo crear el inventario. Verifique los datos proporcionados.");

            // Transform Entity → Resource para la respuesta
            var responseResource = InventoryByProductResourceAssembler.ToResourceFromEntity(result);

            return CreatedAtAction(nameof(GetByProductById), new { id = result.Id }, responseResource);
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"Error de validación: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpGet("by-product")]
    [SwaggerOperation("Listar todos los Inventarios por Producto con filtros", OperationId = "ListInventoryByProduct")]
    [SwaggerResponse(StatusCodes.Status200OK, "Lista obtenida.", typeof(IEnumerable<InventoryByProduct>))]
    public async Task<IActionResult> GetAllByProduct(
        [FromQuery] string? categoria,
        [FromQuery] string? producto,
        [FromQuery] DateTime? fechaEntrada,
        [FromQuery] int? stockMin)
    {
        var entities = await _productQueryService.GetAllAsync();
        var resources = entities.Select(InventoryByProductResourceAssembler.ToResourceFromEntity);

        if (!string.IsNullOrEmpty(categoria))
            resources = resources.Where(x => x.CategoriaNombre == categoria);
        if (!string.IsNullOrEmpty(producto))
            resources = resources.Where(x => x.ProductoNombre == producto);
        if (fechaEntrada.HasValue)
            resources = resources.Where(x => x.FechaEntrada.Date == fechaEntrada.Value.Date);
        if (stockMin.HasValue)
            resources = resources.Where(x => x.StockMinimo <= stockMin.Value);
        return Ok(resources);
    }

    [HttpGet("by-product/{id}")]
    [SwaggerOperation("Obtener Inventario por Producto por ID", OperationId = "GetInventoryByProductById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Inventario encontrado.", typeof(InventoryByProductResource))]
    public async Task<IActionResult> GetByProductById(int id)
    {
        var result = await _productQueryService.GetByIdAsync(id);
        if (result == null) return NotFound();

        // ✅ Transformar Entity a Resource enriquecido
        var resource = InventoryByProductResourceAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    [HttpDelete("by-product/{id}")]
    [SwaggerOperation("Eliminar Inventario por Producto por ID", OperationId = "DeleteInventoryByProductById")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Inventario eliminado.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Inventario no encontrado.")]
    public async Task<IActionResult> DeleteByProductById(int id)
    {
        var deleted = await _productCommandService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }

    // LOTES

    [HttpPost("by-batch")]
    [SwaggerOperation("Crear Inventario por Lote", OperationId = "CreateInventoryByBatch")]
    [SwaggerResponse(StatusCodes.Status201Created, "Inventario por lote creado correctamente.", typeof(InventoryByBatchResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos inválidos proporcionados.")]
    public async Task<IActionResult> CreateByBatch([FromBody] CreateInventoryByBatchResource resource)
    {
        try
        {
            // Transform Resource → Command siguiendo el patrón DDD
            var command = CreateInventoryByBatchCommandAssembler.ToCommandFromResource(resource);

            // Manejar el Command a través del Application Service
            var result = await _batchCommandService.Handle(command);

            if (result == null)
                return BadRequest("No se pudo crear el inventario por lote. Verifique los datos proporcionados.");

            // Transform Entity → Resource para la respuesta
            var responseResource = InventoryByBatchResourceAssembler.ToResource(result);

            return CreatedAtAction(nameof(GetByBatchById), new { id = result.Id }, responseResource);
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"Error de validación: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpGet("by-batch")]
    [SwaggerOperation("Listar todos los Inventarios por Lote con filtros", OperationId = "ListInventoryByBatch")]
    [SwaggerResponse(StatusCodes.Status200OK, "Lista obtenida.", typeof(IEnumerable<InventoryByBatch>))]
    public async Task<IActionResult> GetAllByBatch(
        [FromQuery] string? producto,
        [FromQuery] string? proveedor,
        [FromQuery] DateTime? fechaEntrada,
        [FromQuery] int? cantidad,
        [FromQuery] decimal? precio)
    {
        var entities = await _batchQueryService.Handle(new GetInventoryByBatchQuery());
        var resources = entities.Select(InventoryByBatchResourceAssembler.ToResource);

        if (!string.IsNullOrEmpty(producto))
            resources = resources.Where(x => x.ProductoNombre == producto);
        if (!string.IsNullOrEmpty(proveedor))
            resources = resources.Where(x => x.Proveedor == proveedor);
        if (fechaEntrada.HasValue)
            resources = resources.Where(x => x.FechaEntrada.Date == fechaEntrada.Value.Date);
        if (cantidad.HasValue)
            resources = resources.Where(x => x.Cantidad == cantidad.Value);
        if (precio.HasValue)
            resources = resources.Where(x => x.Precio == precio.Value);
        return Ok(resources);
    }

    [HttpGet("by-batch/{id}")]
    [SwaggerOperation("Obtener Inventario por Lote por ID", OperationId = "GetInventoryByBatchById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Inventario encontrado.", typeof(InventoryByBatchResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Inventario no encontrado.")]
    public async Task<IActionResult> GetByBatchById(int id)
    {
        var result = await _batchQueryService.GetByIdAsync(id);

        if (result == null)
            return NotFound();

        // Transform Entity → Resource para la respuesta
        var resource = InventoryByBatchResourceAssembler.ToResource(result);
        return Ok(resource);
    }

    [HttpDelete("by-batch/{id}")]
    [SwaggerOperation("Eliminar Inventario por Lote por ID", OperationId = "DeleteInventoryByBatchById")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Inventario eliminado.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Inventario no encontrado.")]
    public async Task<IActionResult> DeleteByBatchById(int id)
    {
        var deleted = await _batchCommandService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
