using Lot.AlertStockManagement.Domain.Model.Aggregates;
using Lot.AlertStockManagement.Interfaces.REST.Resources;

namespace Lot.AlertStockManagement.Interfaces.REST.Transform;

public static class StockAlertTransform
{
    public static StockAlertResource ToResource(StockAlertItem item)
    {
        return new StockAlertResource
        {
            ProductName = item.ProductName,
            Quantity = item.Quantity,
            MinStock = item.MinStock,
            EntryDate = item.EntryDate,
            IsLowStock = item.IsLowStock
        };
    }
}