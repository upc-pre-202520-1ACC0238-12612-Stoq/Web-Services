using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.ProductManagement.Domain.Repositories;
using Lot.ProductManagement.Domain.Services;

namespace Lot.ProductManagement.Application.Internal.QueryServices;

public class TagQueryService : ITagQueryService
{
    private readonly ITagRepository _tagRepository;

    public TagQueryService(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        return await _tagRepository.FindAllTagsAsync();
    }
} 