using System.Net.Mime;
using Lot.ProductManagement.Domain.Model.Queries;
using Lot.ProductManagement.Domain.Services;
using Lot.ProductManagement.Interfaces.REST.Resources;
using Lot.ProductManagement.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Lot.IAM.Infrastructure.Authorization;

namespace Lot.ProductManagement.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Puntos de acceso disponibles para la gestión de productos.")]
[AuthorizeRoles("Employee", "Administrator")]
public class ProductsController(
    IProductCommandService productCommandService,
    IProductQueryService productQueryService)
    : ControllerBase
{
    [HttpGet("{productId:int}")]
    [SwaggerOperation("Obtener Producto por Id", "Obtiene un producto por su identificador único.", OperationId = "GetProductById")]
    [SwaggerResponse(200, "El producto fue encontrado y retornado.", typeof(ProductResource))]
    [SwaggerResponse(404, "El producto no fue encontrado.")]
    public async Task<IActionResult> GetProductById(int productId)
    {
        var product = await productQueryService.Handle(new GetProductByIdQuery(productId));
        if (product is null) return NotFound();
        var resource = ProductResourceFromEntityAssembler.ToResourceFromEntity(product);
        return Ok(resource);
    }

    [HttpPost]
    [SwaggerOperation("Crear Producto", "Crea un nuevo producto.", OperationId = "CreateProduct")]
    [SwaggerResponse(201, "El producto fue creado.", typeof(ProductResource))]
    [SwaggerResponse(400, "El producto no fue creado.")]
    public async Task<IActionResult> CreateProduct(CreateProductResource resource)
    {
        try
        {
            var createProductCommand = CreateProductCommandFromResourceAssembler.ToCommandFromResource(resource);
            var product = await productCommandService.Handle(createProductCommand);
            if (product is null) return BadRequest("Error al crear el producto");
            var productResource = ProductResourceFromEntityAssembler.ToResourceFromEntity(product);
            return CreatedAtAction(nameof(GetProductById), new { productId = product.Id }, productResource);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }



    [HttpGet]
    [SwaggerOperation("Obtener Todos los Productos", "Obtiene todos los productos.", OperationId = "GetAllProducts")]
    [SwaggerResponse(200, "Los productos fueron encontrados y retornados.", typeof(IEnumerable<ProductResource>))]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await productQueryService.Handle(new GetAllProductsQuery());
        var productResources = products.Select(ProductResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(productResources);
    }

    [HttpGet("by-category/{categoryId:int}")]
    [SwaggerOperation("Obtener Productos por Categoría", "Obtiene productos filtrados por categoría.", OperationId = "GetProductsByCategory")]
    [SwaggerResponse(200, "Productos encontrados.", typeof(IEnumerable<ProductResource>))]
    public async Task<IActionResult> GetProductsByCategory(int categoryId)
    {
        var products = await productQueryService.Handle(new GetProductsByCategoryQuery(categoryId));
        var resources = products.Select(ProductResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("by-tag/{tagId:int}")]
    [SwaggerOperation("Obtener Productos por Etiqueta", "Obtiene productos filtrados por etiqueta.", OperationId = "GetProductsByTag")]
    [SwaggerResponse(200, "Productos encontrados.", typeof(IEnumerable<ProductResource>))]
    public async Task<IActionResult> GetProductsByTag(int tagId)
    {
        var products = await productQueryService.Handle(new GetProductsByTagQuery(tagId));
        var resources = products.Select(ProductResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }


} 