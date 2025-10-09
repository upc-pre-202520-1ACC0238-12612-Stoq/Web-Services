namespace Lot.ProductManagement.Interfaces.REST.Resources;

/// <summary>
/// Recurso de etiqueta para la API REST
/// </summary>
/// <param name="Id">
/// Identificador único de la etiqueta
/// </param>
/// <param name="Name">
/// Nombre de la etiqueta
/// </param>
public record TagResource(
    int Id,
    string Name
); 