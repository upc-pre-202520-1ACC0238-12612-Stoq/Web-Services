namespace Lot.ProductManagement.Interfaces.REST.Resources;

/// <summary>
/// Recurso de combo para la API REST
/// </summary>
public record ComboResource(
    int Id,
    string Name,
    List<ComboItemResource> Items
);

/// <summary>
/// Recurso de ítem de combo para la API REST
/// </summary>
public record ComboItemResource(
    int Id,
    int ProductId,
    string ProductName,
    string ProductDescription,
    decimal ProductPrice,
    int Quantity
);