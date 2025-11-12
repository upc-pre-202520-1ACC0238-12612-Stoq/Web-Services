namespace Lot.AlertStockManagement.Interfaces.REST.Resources;

public class StockAlertResource
{
    public string ProductName { get; set; } = null!;
    public string CategoryName { get; set; } = null!;
    public int Quantity { get; set; }
    public int MinStock { get; set; }
    public decimal Price { get; set; }
    public DateTime EntryDate { get; set; }
    public int StockDeficit { get; set; }
    public string AlertLevel { get; set; } = null!;
}