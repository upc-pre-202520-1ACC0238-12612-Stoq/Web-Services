namespace Lot.ProductManagement.Domain.Model.Commands;

public record UpdateComboCommand(
    int Id,
    string Name,
    List<ComboItemCommand> Items
);