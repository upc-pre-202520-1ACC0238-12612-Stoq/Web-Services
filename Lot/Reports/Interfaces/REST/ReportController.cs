using System.Net.Mime;
using Lot.Reports.Domain.Services;
using Lot.Reports.Domain.Model.Queries;
using Lot.Reports.Domain.Model.Commands;
using Lot.Reports.Interfaces.REST.Resources;
using Lot.Reports.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Lot.IAM.Infrastructure.Authorization;

namespace Lot.Reports.Interfaces.REST;

[ApiController]
[Route("api/v1/reports")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Puntos de acceso disponibles para la gestión de reportes de categoría y de stock promedio.")]
[AuthorizeRoles("Administrator")]
public class ReportController(
    ICategoryReportCommandService categoryCmdService,
    ICategoryReportQueryService categoryQryService,
    IStockAverageReportCommandService stockCmdService,
    IStockAverageReportQueryService stockQryService)
    : ControllerBase
{
   

    [HttpGet]
    [SwaggerOperation("Obtener todos los reportes generales", "Devuelve todos los reportes disponibles, tanto de categoría como de stock promedio.", OperationId = "GetAllReports")]
    [SwaggerResponse(StatusCodes.Status200OK, "Reportes encontrados y retornados.", typeof(GeneralReportResource))]
    public async Task<IActionResult> GetAllReports()
    {
        var categoryReports = await categoryQryService.Handle(new GetAllCategoryReportsQuery());
        var stockReports = await stockQryService.Handle(new GetAllStockAverageReportsQuery());

        var response = new GeneralReportResource
        {
            CategoryReports = categoryReports.Select(CategoryReportResourceAssembler.ToResource).ToList(),
            StockAverageReports = stockReports.Select(StockAverageReportResourceAssembler.ToResource).ToList()
        };

        return Ok(response);
    }


    [HttpPost("category")]
    [SwaggerOperation("Crear CategoryReport", "Crea un nuevo reporte de categoría.", OperationId = "CreateCategoryReport")]
    [SwaggerResponse(StatusCodes.Status200OK, "Reporte de categoría creado.", typeof(CategoryReportResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos inválidos.")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryReportResource resource)
    {
        var command = CreateCategoryReportCommandAssembler.ToCommand(resource);
        var result = await categoryCmdService.Handle(command);

        return result is null
            ? BadRequest("No se pudo crear el reporte de categoría.")
            : Ok(CategoryReportResourceAssembler.ToResource(result));
    }

    [HttpGet("category")]
    [SwaggerOperation("Listar CategoryReports", "Devuelve todos los reportes de categoría.", OperationId = "GetAllCategoryReports")]
    [SwaggerResponse(StatusCodes.Status200OK, "Reportes de categoría encontrados.", typeof(IEnumerable<CategoryReportResource>))]
    public async Task<IActionResult> GetAllCategoryReports()
    {
        var result = await categoryQryService.Handle(new GetAllCategoryReportsQuery());
        return Ok(result.Select(CategoryReportResourceAssembler.ToResource));
    }

    [HttpGet("category/{id:int}")]
    [SwaggerOperation("Obtener CategoryReport por ID", "Devuelve un reporte de categoría específico por su ID.", OperationId = "GetCategoryReportById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Reporte encontrado.", typeof(CategoryReportResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Reporte no encontrado.")]
    public async Task<IActionResult> GetCategoryReportById([FromRoute] int id)
    {
        var result = await categoryQryService.Handle(new GetCategoryReportByIdQuery(id));
        return result is null
            ? NotFound("El reporte de categoría no fue encontrado.")
            : Ok(CategoryReportResourceAssembler.ToResource(result));
    }

    [HttpGet("category/by-date")]
    [SwaggerOperation("Filtrar CategoryReports por fecha", "Devuelve los reportes de categoría para una fecha específica.", OperationId = "GetCategoryReportsByDate")]
    [SwaggerResponse(StatusCodes.Status200OK, "Reportes encontrados.", typeof(IEnumerable<CategoryReportResource>))]
    public async Task<IActionResult> GetCategoryByDate([FromQuery] DateTime date)
    {
        var result = await categoryQryService.Handle(new GetCategoryReportsByDateQuery(date));
        return Ok(result.Select(CategoryReportResourceAssembler.ToResource));
    }


    [HttpPost("stock-average")]
    [SwaggerOperation("Crear StockAverageReport", "Crea un nuevo reporte de stock promedio.", OperationId = "CreateStockAverageReport")]
    [SwaggerResponse(StatusCodes.Status200OK, "Reporte de stock promedio creado.", typeof(StockAverageReportResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos inválidos.")]
    public async Task<IActionResult> CreateStockAverage([FromBody] CreateStockAverageReportResource resource)
    {
        var command = CreateStockAverageReportCommandAssembler.ToCommand(resource);
        var result = await stockCmdService.Handle(command);

        return result is null
            ? BadRequest("No se pudo crear el reporte de stock promedio.")
            : Ok(StockAverageReportResourceAssembler.ToResource(result));
    }

    [HttpGet("stock-average")]
    [SwaggerOperation("Listar StockAverageReports", "Devuelve todos los reportes de stock promedio.", OperationId = "GetAllStockAverageReports")]
    [SwaggerResponse(StatusCodes.Status200OK, "Reportes encontrados.", typeof(IEnumerable<StockAverageReportResource>))]
    public async Task<IActionResult> GetAllStockAverageReports()
    {
        var result = await stockQryService.Handle(new GetAllStockAverageReportsQuery());
        return Ok(result.Select(StockAverageReportResourceAssembler.ToResource));
    }

    [HttpGet("stock-average/{id:int}")]
    [SwaggerOperation("Obtener StockAverageReport por ID", "Devuelve un reporte de stock promedio específico por su ID.", OperationId = "GetStockAverageReportById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Reporte encontrado.", typeof(StockAverageReportResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Reporte no encontrado.")]
    public async Task<IActionResult> GetStockAverageReportById([FromRoute] int id)
    {
        var result = await stockQryService.Handle(new GetStockAverageReportByIdQuery(id));
        return result is null
            ? NotFound("El reporte de stock promedio no fue encontrado.")
            : Ok(StockAverageReportResourceAssembler.ToResource(result));
    }

    [HttpGet("stock-average/by-date")]
    [SwaggerOperation("Filtrar StockAverageReports por fecha", "Devuelve los reportes de stock promedio para una fecha específica.", OperationId = "GetStockAverageReportsByDate")]
    [SwaggerResponse(StatusCodes.Status200OK, "Reportes encontrados.", typeof(IEnumerable<StockAverageReportResource>))]
    public async Task<IActionResult> GetStockAverageByDate([FromQuery] DateTime date)
    {
        var result = await stockQryService.Handle(new GetStockAverageReportsByDateQuery(date));
        return Ok(result.Select(StockAverageReportResourceAssembler.ToResource));
    }
}
