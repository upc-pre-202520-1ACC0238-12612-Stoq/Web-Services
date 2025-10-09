//L
using Lot.Inventaries.Domain.Model.Aggregates;
using Lot.Inventaries.Domain.Repositories;
using Lot.Shared.Infraestructure.Persistence.EFC.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;


namespace Lot.Inventaries.Infraestructure.Persistence.EFC.Repositories
{
    public class InventoryByBatchRepository : IInventoryByBatchRepository
    {
        private readonly AppDbContext _context;

        public InventoryByBatchRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InventoryByBatch>> ListAsync()
        {
            return await _context.Set<InventoryByBatch>().ToListAsync();
        }

        public async Task AddAsync(InventoryByBatch batch)
        {
            await _context.Set<InventoryByBatch>().AddAsync(batch);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<InventoryByBatch>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<InventoryByBatch>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }

}
