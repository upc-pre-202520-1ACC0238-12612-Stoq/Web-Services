using Lot.Sales.Domain.Model.Aggregates;
using Lot.Sales.Domain.Repositories;
using Lot.Shared.Infraestructure.Persistence.EFC.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Lot.Sales.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
/// Repositorio EF Core para ventas con métodos de búsqueda especializados.
/// Implementa el patrón Repository para el agregado Sale.
/// </summary>
public class SaleRepository : ISaleRepository
{
    private readonly AppDbContext _context;

    public SaleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Sale>> ListAsync()
    {
        return await _context.Set<Sale>()
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();
    }

    public async Task<Sale?> FindByIdAsync(int id)
    {
        return await _context.Set<Sale>()
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task AddAsync(Sale entity)
    {
        await _context.Set<Sale>().AddAsync(entity);
    }

    public async Task UpdateAsync(Sale entity)
    {
        _context.Set<Sale>().Update(entity);
    }

    public async Task DeleteAsync(Sale entity)
    {
        _context.Set<Sale>().Remove(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await FindByIdAsync(id);
        if (entity != null)
        {
            await DeleteAsync(entity);
        }
    }

    // Métodos especializados del repositorio de ventas

    public async Task<IEnumerable<Sale>> GetByProductIdAsync(int productId)
    {
        return await _context.Set<Sale>()
            .Where(s => s.ProductId == productId)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Set<Sale>()
            .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Sale>> GetByCustomerNameAsync(string customerName)
    {
        return await _context.Set<Sale>()
            .Where(s => s.CustomerName.Contains(customerName))
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Sale>> GetRecentSalesAsync(int limit = 10)
    {
        return await _context.Set<Sale>()
            .OrderByDescending(s => s.SaleDate)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalSalesByDateAsync(DateTime date)
    {
        var startDate = date.Date;
        var endDate = date.Date.AddDays(1);

        return await _context.Set<Sale>()
            .Where(s => s.SaleDate >= startDate && s.SaleDate < endDate)
            .SumAsync(s => s.TotalAmount);
    }

    public async Task<int> GetTotalQuantitySoldByProductAsync(int productId)
    {
        return await _context.Set<Sale>()
            .Where(s => s.ProductId == productId)
            .SumAsync(s => s.Quantity.Value);
    }
}