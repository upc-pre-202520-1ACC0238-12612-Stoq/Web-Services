namespace Lot.ProductManagement.Interfaces.REST.Resources;

/// <summary>
/// Recurso de categoría para la API REST
/// </summary>
/// <param name="Id">
/// Identificador único de la categoría
/// </param>
/// <param name="Name">
/// Nombre de la categoría
/// </param>
public record CategoryResource(
    int Id,
    string Name
); 