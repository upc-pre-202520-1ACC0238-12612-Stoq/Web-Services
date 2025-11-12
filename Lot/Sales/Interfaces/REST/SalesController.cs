using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using Lot.Sales.Domain.Model.Aggregates;
using Lot.Sales.Domain.Services;
using Lot.Sales.Interfaces.REST.Resources;
using Lot.Sales.Interfaces.REST.Transform;
using Lot.IAM.Infrastructure.Authorization;
using Lot.Inventaries.Domain.Services;
using Lot.Inventaries.Domain.Repositories;
using Lot.Shared.Infraestructure.Persistence.EFC.Configuration.Extensions;

namespace Lot.Sales.Interfaces.REST;

[ApiController]
[Route("api/v1/sales")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Operaciones de ventas con integración automática de inventario y reportes.")]
[AuthorizeRoles("Administrator", "Employee")]
public class SalesController : ControllerBase
{
    private readonly ISaleCommandService _saleCommandService;
    private readonly IInventoryByProductQueryService _inventoryQueryService;
    private readonly AppDbContext _context;

    public SalesController(
        ISaleCommandService saleCommandService,
        IInventoryByProductQueryService inventoryQueryService,
        AppDbContext context)
    {
        _saleCommandService = saleCommandService;
        _inventoryQueryService = inventoryQueryService;
        _context = context;
    }

    /// <summary>
    /// Crea una nueva venta con reducción automática de inventario y generación de reporte
    /// </summary>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Realizar venta con stock automático",
        Description = "Crea una venta, reduce el inventario automáticamente y genera reporte.",
        OperationId = "CreateSale")]
    [SwaggerResponse(StatusCodes.Status201Created, "Venta realizada correctamente.", typeof(SaleResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos inválidos o stock insuficiente.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Producto no encontrado.")]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleResource resource)
    {
        try
        {
            // Transform Resource → Command siguiendo el patrón DDD
            var command = CreateSaleCommandAssembler.ToCommandFromResource(resource);

            // Manejar el Command con integración automática de inventario
            var result = await _saleCommandService.Handle(command);

            if (result == null)
                return BadRequest("No se pudo realizar la venta. Verifique el stock disponible y los datos proporcionados.");

            // Transform Entity → Resource para la respuesta
            var responseResource = SaleResourceAssembler.ToResourceFromEntity(result);

            return CreatedAtAction(nameof(GetSaleById), new { id = result.Id }, responseResource);
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

    /// <summary>
    /// Obtiene una venta por su ID
    /// </summary>
    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Obtener venta por ID",
        Description = "Recupera los detalles de una venta específica.",
        OperationId = "GetSaleById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Venta encontrada.", typeof(SaleResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Venta no encontrada.")]
    public async Task<IActionResult> GetSaleById(int id)
    {
        // Por ahora implementado con repositorio directo (necesitaríamos QueryService)
        return Ok($"Venta ID {id} - Endpoint por implementar con QueryService");
    }

    /// <summary>
    /// Endpoint para verificar stock disponible en tiempo real
    /// </summary>
    [HttpGet("check-stock/{productId}")]
    [SwaggerOperation(
        Summary = "Verificar stock disponible",
        Description = "Verifica el stock disponible actual para un producto antes de vender.",
        OperationId = "CheckAvailableStock")]
    [SwaggerResponse(StatusCodes.Status200OK, "Stock disponible.", typeof(object))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Producto o inventario no encontrado.")]
    public async Task<IActionResult> CheckAvailableStock(int productId)
    {
        try
        {
            // 1. Obtener información del producto
            var product = await _context.Set<Lot.ProductManagement.Domain.Model.Aggregates.Product>()
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                return NotFound(new {
                    Message = $"Producto con ID {productId} no encontrado",
                    AvailableStock = 0,
                    ProductName = "No encontrado"
                });
            }

            // 2. Obtener inventario actual del producto
            var inventory = await _context.Set<Lot.Inventaries.Domain.Model.Aggregates.InventoryByProduct>()
                .FirstOrDefaultAsync(i => i.ProductoId == productId);

            if (inventory == null)
            {
                return NotFound(new {
                    Message = $"No existe inventario para el producto {product.Name}",
                    AvailableStock = 0,
                    ProductName = product.Name,
                    HasInventory = false
                });
            }

            // 3. Calcular información adicional
            var stockStatus = inventory.Cantidad <= inventory.StockMinimo ? "BAJO" : "NORMAL";
            var canSell = inventory.Cantidad > 0;

            return Ok(new {
                Message = $"Stock disponible para producto {product.Name}",
                AvailableStock = inventory.Cantidad,
                ProductName = product.Name,
                StockMinimo = inventory.StockMinimo,
                StockStatus = stockStatus,
                CanSell = canSell,
                LastUpdated = inventory.FechaEntrada,
                Price = inventory.Precio,
                ProductId = productId,
                HasInventory = true
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {
                Message = $"Error al verificar stock: {ex.Message}",
                AvailableStock = 0,
                ProductName = "Error",
                Error = true
            });
        }
    }
}