using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.ProductManagement.Domain.Repositories;
using Lot.Shared.Infraestructure.Persistence.EFC.Configuration.Extensions;
using Lot.Shared.Infraestructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lot.ProductManagement.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
/// Tag repository implementation
/// </summary>
public class TagRepository(AppDbContext context) : BaseRepository<Tag>(context), ITagRepository
{
    public async Task<Tag?> FindTagByNameAsync(string name)
    {
        return await Context.Set<Tag>()
            .FirstOrDefaultAsync(t => t.Name == name);
    }

    public async Task<IEnumerable<Tag>> FindAllTagsAsync()
    {
        return await Context.Set<Tag>()
            .ToListAsync();
    }

    public async Task<IEnumerable<Tag>> FindTagsByIdsAsync(List<int> tagIds)
    {
        return await Context.Set<Tag>()
            .Where(t => tagIds.Contains(t.Id))
            .ToListAsync();
    }
} 