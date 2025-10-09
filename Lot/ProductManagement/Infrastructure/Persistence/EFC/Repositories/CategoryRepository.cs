using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.ProductManagement.Domain.Repositories;
using Lot.Shared.Infraestructure.Persistence.EFC.Configuration.Extensions;
using Lot.Shared.Infraestructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lot.ProductManagement.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
/// Category repository implementation
/// </summary>
public class CategoryRepository(AppDbContext context) : BaseRepository<Category>(context), ICategoryRepository
{
    public async Task<Category?> FindCategoryByNameAsync(string name)
    {
        return await Context.Set<Category>()
            .FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task<IEnumerable<Category>> FindAllCategoriesAsync()
    {
        return await Context.Set<Category>()
            .ToListAsync();
    }
} 