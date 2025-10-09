namespace Lot.AlertStockManagement.Domain.Model.Aggregates;

public class StockAlertItem
{
    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }
    public int MinStock { get; set; }
    public DateTime EntryDate { get; set; }
    public bool IsLowStock { get; set; }
}