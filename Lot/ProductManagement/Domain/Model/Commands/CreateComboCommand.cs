namespace Lot.ProductManagement.Domain.Model.Commands;

public record CreateComboCommand(
    string Name,
    List<ComboItemCommand> Items
);

public record ComboItemCommand(
    int ProductId,
    int Quantity
);