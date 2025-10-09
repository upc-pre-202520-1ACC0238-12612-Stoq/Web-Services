using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.ProductManagement.Domain.Repositories;
using Lot.Shared.Infraestructure.Persistence.EFC.Configuration.Extensions;
using Lot.Shared.Infraestructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lot.ProductManagement.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
/// Product repository implementation
/// </summary>
public class ProductRepository(AppDbContext context) : BaseRepository<Product>(context), IProductRepository
{
    public async Task<Product?> FindProductByNameAsync(string name)
    {
        return await Context.Set<Product>()
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .Include(p => p.ProductTags)
            .ThenInclude(pt => pt.Tag)
            .FirstOrDefaultAsync(p => p.Name == name);
    }

    public async Task<IEnumerable<Product>> FindAllProductsAsync()
    {
        return await Context.Set<Product>()
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .Include(p => p.ProductTags)
            .ThenInclude(pt => pt.Tag)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> FindProductsByCategoryAsync(int categoryId)
    {
        return await Context.Set<Product>()
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .Include(p => p.ProductTags)
            .ThenInclude(pt => pt.Tag)
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> FindProductsByTagAsync(int tagId)
    {
        return await Context.Set<Product>()
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .Include(p => p.ProductTags)
            .ThenInclude(pt => pt.Tag)
            .Where(p => p.ProductTags.Any(pt => pt.TagId == tagId))
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> FindProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        return await Context.Set<Product>()
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .Include(p => p.ProductTags)
            .ThenInclude(pt => pt.Tag)
            .Where(p => p.SalePrice >= minPrice && p.SalePrice <= maxPrice)
            .ToListAsync();
    }

    public async Task<Product?> FindProductWithRelationsAsync(int id)
    {
        return await Context.Set<Product>()
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .Include(p => p.ProductTags)
            .ThenInclude(pt => pt.Tag)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
} 