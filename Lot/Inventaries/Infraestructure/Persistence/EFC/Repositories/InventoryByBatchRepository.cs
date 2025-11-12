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
            // MEJORADO: Cargar relaciones para datos enriquecidos
            return await _context.Set<InventoryByBatch>()
                .Include(b => b.Product)
                .ThenInclude(p => p.Category)
                .Include(b => b.Unit)  // ✅ Carga directa la unidad del lote
                .ToListAsync();
        }

        public async Task<InventoryByBatch?> FindByIdAsync(int id)
        {
            return await _context.Set<InventoryByBatch>()
                .Include(b => b.Product)
                .ThenInclude(p => p.Category)
                .Include(b => b.Unit)  // ✅ Carga directa la unidad del lote
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        // NUEVO: Método para obtener con relaciones (para enriquecer responses)
        public async Task<InventoryByBatch?> FindByIdWithRelationsAsync(int id)
        {
            var entity = await _context.Set<InventoryByBatch>()
                .Include(b => b.Product)
                .ThenInclude(p => p.Category)
                .Include(b => b.Unit)  // ✅ Carga directa la unidad del lote
                .FirstOrDefaultAsync(b => b.Id == id);


            return entity;
        }

        public async Task AddAsync(InventoryByBatch batch)
        {
            await _context.Set<InventoryByBatch>().AddAsync(batch);
            // FIXED: No SaveChangesAsync aquí - el UnitOfWork se encarga
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<InventoryByBatch>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<InventoryByBatch>().Remove(entity);
                // FIXED: No SaveChangesAsync aquí - el UnitOfWork se encarga
            }
        }

        // NUEVO: Método para verificar si existe un producto
        public async Task<bool> ProductoExistsAsync(int productoId)
        {
            return await _context.Set<Lot.ProductManagement.Domain.Model.Aggregates.Product>()
                .AnyAsync(p => p.Id == productoId);
        }

        // NUEVO: Obtener productos para validación
        public async Task<Lot.ProductManagement.Domain.Model.Aggregates.Product?> GetProductByIdAsync(int productoId)
        {
            return await _context.Set<Lot.ProductManagement.Domain.Model.Aggregates.Product>()
                .Include(p => p.Category)
                .Include(p => p.Unit)
                .FirstOrDefaultAsync(p => p.Id == productoId);
        }

        // NUEVOS: Métodos de búsqueda por nombre
        public async Task<Lot.ProductManagement.Domain.Model.Aggregates.Product?> FindProductByNameAsync(string nombre, string? categoria = null)
        {
            var query = _context.Set<Lot.ProductManagement.Domain.Model.Aggregates.Product>()
                .Include(p => p.Category)
                .Include(p => p.Unit)
                .AsQueryable();

            // Búsqueda exacta primero
            query = query.Where(p => p.Name.ToLower() == nombre.ToLower());

            // Filtrar por categoría si se proporciona
            if (!string.IsNullOrWhiteSpace(categoria))
            {
                query = query.Where(p => p.Category.Name.ToLower() == categoria.ToLower());
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Lot.ProductManagement.Domain.Model.Aggregates.Product>> SearchProductsByNameAsync(string nombre, string? categoria = null)
        {
            var query = _context.Set<Lot.ProductManagement.Domain.Model.Aggregates.Product>()
                .Include(p => p.Category)
                .Include(p => p.Unit)
                .AsQueryable();

            // Búsqueda parcial (contiene)
            query = query.Where(p => p.Name.ToLower().Contains(nombre.ToLower()));

            // Filtrar por categoría si se proporciona
            if (!string.IsNullOrWhiteSpace(categoria))
            {
                query = query.Where(p => p.Category.Name.ToLower() == categoria.ToLower());
            }

            return await query.ToListAsync();
        }
    }
}
