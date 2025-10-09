namespace Lot.AlertStockManagement.Interfaces.REST.Resources;

public class StockAlertResource
{
    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }
    public int MinStock { get; set; }
    public DateTime EntryDate { get; set; }
    public bool IsLowStock { get; set; }
}