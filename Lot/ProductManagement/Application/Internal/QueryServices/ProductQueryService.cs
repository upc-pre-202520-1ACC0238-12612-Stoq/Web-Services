using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.ProductManagement.Domain.Model.Queries;
using Lot.ProductManagement.Domain.Repositories;
using Lot.ProductManagement.Domain.Services;

namespace Lot.ProductManagement.Application.Internal.QueryServices;

/// <summary>
/// Product query service implementation
/// </summary>
public class ProductQueryService(IProductRepository productRepository) : IProductQueryService
{
    public async Task<Product?> Handle(GetProductByIdQuery query)
    {
        return await productRepository.FindProductWithRelationsAsync(query.Id);
    }

    public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery query)
    {
        return await productRepository.FindAllProductsAsync();
    }

    public async Task<IEnumerable<Product>> Handle(GetProductsByCategoryQuery query)
    {
        return await productRepository.FindProductsByCategoryAsync(query.CategoryId);
    }

    public async Task<IEnumerable<Product>> Handle(GetProductsByTagQuery query)
    {
        return await productRepository.FindProductsByTagAsync(query.TagId);
    }

    public async Task<IEnumerable<Product>> Handle(GetProductsByPriceRangeQuery query)
    {
        return await productRepository.FindProductsByPriceRangeAsync(query.MinPrice, query.MaxPrice);
    }
} 