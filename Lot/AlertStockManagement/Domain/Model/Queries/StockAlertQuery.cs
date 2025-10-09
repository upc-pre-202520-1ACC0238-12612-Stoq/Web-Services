namespace Lot.AlertStockManagement.Domain.Model.Queries;

public class StockAlertQuery
{
    public bool IncludeLowStock { get; set; } = true;
}