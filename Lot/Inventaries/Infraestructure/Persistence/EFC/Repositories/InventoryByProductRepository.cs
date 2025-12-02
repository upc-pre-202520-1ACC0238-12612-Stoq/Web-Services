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
            // MEJORADO: Cargar relaciones para datos enriquecidos
            return await _context.Set<InventoryByProduct>()
                .Include(p => p.Product)
                .ThenInclude(p => p.Category)
                .Include(p => p.Product)
                .ThenInclude(p => p.Unit)
                .ToListAsync();
        }

        public async Task<InventoryByProduct?> FindByIdAsync(int id)
        {
            return await _context.Set<InventoryByProduct>()
                .Include(p => p.Product)
                .ThenInclude(p => p.Category)
                .Include(p => p.Product)
                .ThenInclude(p => p.Unit)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // NUEVO: Método para obtener con relaciones (para enriquecer responses)
        public async Task<InventoryByProduct?> FindByIdWithRelationsAsync(int id)
        {
            var entity = await _context.Set<InventoryByProduct>()
                .Include(p => p.Product)
                .ThenInclude(p => p.Category)
                .Include(p => p.Product)
                .ThenInclude(p => p.Unit)
                .FirstOrDefaultAsync(p => p.Id == id);

        
            return entity;
        }

        public async Task AddAsync(InventoryByProduct product)
        {
            await _context.Set<InventoryByProduct>().AddAsync(product);
            // FIXED: No SaveChangesAsync aquí - el UnitOfWork se encarga
        }

        public async Task UpdateAsync(InventoryByProduct product)
        {
            _context.Set<InventoryByProduct>().Update(product);
            // FIXED: No SaveChangesAsync aquí - el UnitOfWork se encarga
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<InventoryByProduct>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<InventoryByProduct>().Remove(entity);
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