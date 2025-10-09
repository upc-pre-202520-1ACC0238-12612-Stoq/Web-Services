namespace Lot.ProductManagement.Interfaces.REST.Resources;

/// <summary>
/// Recurso de unidad de medida para la API REST
/// </summary>
/// <param name="Id">
/// Identificador único de la unidad
/// </param>
/// <param name="Name">
/// Nombre de la unidad
/// </param>
/// <param name="Abbreviation">
/// Abreviación de la unidad
/// </param>
public record UnitResource(
    int Id,
    string Name,
    string Abbreviation
); 