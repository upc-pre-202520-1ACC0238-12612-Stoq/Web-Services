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
            Productos = products.Select(p => InventoryByProductResourceAssembler.ToCommandFromResource(p)).ToList(),
            Lotes = batches.Select(b => InventoryByBatchResourceAssembler.ToResource(b)).ToList(),
        };

        return Ok(response);
    }


    // PRODUCTOS

    [HttpPost("by-product")]
    [SwaggerOperation("Crear Inventario por Producto", OperationId = "CreateInventoryByProduct")]
    [SwaggerResponse(StatusCodes.Status200OK, "Creado correctamente.", typeof(InventoryByProduct))]
    public async Task<IActionResult> CreateByProduct([FromBody] CreateInventoryByProductCommand command)
    {
        var result = await _productCommandService.Handle(command);
        return Ok(result);
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
        var result = await _productQueryService.GetAllAsync();
        if (!string.IsNullOrEmpty(categoria))
            result = result.Where(x => x.Categoria == categoria);
        if (!string.IsNullOrEmpty(producto))
            result = result.Where(x => x.Producto == producto);
        if (fechaEntrada.HasValue)
            result = result.Where(x => x.FechaEntrada.Date == fechaEntrada.Value.Date);
        if (stockMin.HasValue)
            result = result.Where(x => x.StockMinimo <= stockMin.Value);
        return Ok(result);
    }

    [HttpGet("by-product/{id}")]
    [SwaggerOperation("Obtener Inventario por Producto por ID", OperationId = "GetInventoryByProductById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Inventario encontrado.", typeof(InventoryByProduct))]
    public async Task<IActionResult> GetByProductById(int id)
    {
        var result = await _productQueryService.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
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
    [SwaggerResponse(StatusCodes.Status200OK, "Creado correctamente.", typeof(InventoryByBatch))]
    public async Task<IActionResult> CreateByBatch([FromBody] CreateInventoryByBatchCommand command)
    {
        var result = await _batchCommandService.Handle(command);
        return Ok(result);
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
        var result = await _batchQueryService.Handle(new GetInventoryByBatchQuery());
        if (!string.IsNullOrEmpty(producto))
            result = result.Where(x => x.Producto == producto);
        if (!string.IsNullOrEmpty(proveedor))
            result = result.Where(x => x.Proveedor == proveedor);
        if (fechaEntrada.HasValue)
            result = result.Where(x => x.FechaEntrada.Date == fechaEntrada.Value.Date);
        if (cantidad.HasValue)
            result = result.Where(x => x.Cantidad == cantidad.Value);
        if (precio.HasValue)
            result = result.Where(x => x.Precio == precio.Value);
        return Ok(result);
    }

    [HttpGet("by-batch/{id}")]
    [SwaggerOperation("Obtener Inventario por Lote por ID", OperationId = "GetInventoryByBatchById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Inventario encontrado.", typeof(InventoryByBatch))]
    public async Task<IActionResult> GetByBatchById(int id)
    {
        var list = await _batchQueryService.Handle(new GetInventoryByBatchQuery());
        var found = list.FirstOrDefault(i => i.Id == id);
        return found == null ? NotFound() : Ok(found);
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
