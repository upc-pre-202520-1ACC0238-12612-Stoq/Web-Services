using Lot.Inventaries.Domain.Model.Aggregates;
using Lot.Inventaries.Domain.Repositories;
using Lot.Shared.Infraestructure.Persistence.EFC.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Lot.Inventaries.Infraestructure.Persistence.EFC.Repositories
{
    public class InventoryByProductRepository : IInventoryByProductRepository
    {
        private readonly AppDbContext _context;

        public InventoryByProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InventoryByProduct>> ListAsync()
        {
            return await _context.Set<InventoryByProduct>().ToListAsync();
        }

        public async Task<InventoryByProduct?> FindByIdAsync(int id)
        {
            return await _context.Set<InventoryByProduct>().FindAsync(id);
        }

        public async Task AddAsync(InventoryByProduct product)
        {
            await _context.Set<InventoryByProduct>().AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<InventoryByProduct>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<InventoryByProduct>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}