using Lot.AlertStockManagement.Domain.Model.Queries;
using Lot.AlertStockManagement.Domain.Model.Aggregates;
using Lot.AlertStockManagement.Domain.Repositories;
using Lot.Shared.Infraestructure.Persistence.EFC.Configuration.Extensions;
using Lot.Inventaries.Domain.Model.Aggregates; 
using Microsoft.EntityFrameworkCore;


namespace Lot.AlertStockManagement.Infraestructure.Persistence.EFC.Repositories; 
public class InventoryReadRepository : IInventoryReadRepository
{
    private readonly AppDbContext _context;

    public InventoryReadRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<StockAlertItem>> GetStockAlertsAsync(StockAlertQuery query)
    {
        var alerts = await _context.Set<InventoryByProduct>()
            .Where(i => query.IncludeLowStock && i.Cantidad <= i.StockMinimo)
            .Select(i => new StockAlertItem
            {
                ProductName = i.Producto,  // si es string
                Quantity = i.Cantidad,
                MinStock = i.StockMinimo,
                EntryDate = i.FechaEntrada,
                IsLowStock = i.Cantidad <= i.StockMinimo
            })
            .ToListAsync();

        return alerts;
    }
}