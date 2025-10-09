using System.Net.Mime;
using Lot.ProductManagement.Domain.Services;
using Lot.ProductManagement.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Lot.IAM.Infrastructure.Authorization;

namespace Lot.ProductManagement.Interfaces.REST;

[ApiController]
[Route("api/v1/tags")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Puntos de acceso para la gestión de etiquetas.")]
[AuthorizeRoles("Employee", "Administrator")]
public class TagsController : ControllerBase
{
    private readonly ITagQueryService _tagQueryService;

    public TagsController(ITagQueryService tagQueryService)
    {
        _tagQueryService = tagQueryService;
    }

    [HttpGet]
    [SwaggerOperation("Obtener todas las etiquetas", OperationId = "GetAllTags")]
    [SwaggerResponse(200, "Etiquetas encontradas.", typeof(IEnumerable<TagResource>))]
    public async Task<IActionResult> GetAllTags()
    {
        var tags = await _tagQueryService.GetAllAsync();
        var resources = tags.Select(t => new TagResource(t.Id, t.Name));
        return Ok(resources);
    }
} 