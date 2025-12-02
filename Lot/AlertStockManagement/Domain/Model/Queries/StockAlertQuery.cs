namespace Lot.AlertStockManagement.Domain.Model.Queries;

public class StockAlertQuery
{
    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}