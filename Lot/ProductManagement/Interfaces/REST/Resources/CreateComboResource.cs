namespace Lot.ProductManagement.Interfaces.REST.Resources;

/// <summary>
/// Recurso para crear un combo
/// </summary>
public record CreateComboResource(
    string Name,
    List<CreateComboItemResource> Items
);

/// <summary>
/// Recurso para crear un ítem de combo
/// </summary>
public record CreateComboItemResource(
    int ProductId,
    int Quantity
);