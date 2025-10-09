using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.ProductManagement.Domain.Model.Commands;
using Lot.ProductManagement.Domain.Repositories;
using Lot.ProductManagement.Domain.Services;
using Lot.Shared.Domain.Repositories;

namespace Lot.ProductManagement.Application.Internal.CommandServices;

/// <summary>
/// Product command service implementation
/// </summary>
public class ProductCommandService(
    IProductRepository productRepository,
    ITagRepository tagRepository,
    IUnitOfWork unitOfWork) : IProductCommandService
{
    public async Task<Product?> Handle(CreateProductCommand command)
    {
        try
        {
            var product = new Product(command);
            
            // Primero guardamos el producto
            await productRepository.AddAsync(product);
            await unitOfWork.CompleteAsync();
            
            // Luego agregamos las etiquetas si existen
            if (command.TagIds.Any())
            {
                var tags = await tagRepository.FindTagsByIdsAsync(command.TagIds);
                foreach (var tag in tags)
                {
                    product.AddTag(tag.Id);
                }
                productRepository.Update(product);
                await unitOfWork.CompleteAsync();
            }
            
            return product;
        }
        catch (Exception ex)
        {
            // Log the exception for debugging
            Console.WriteLine($"Error creating product: {ex.Message}");
            return null;
        }
    }

    public async Task<Product?> Handle(UpdateProductCommand command)
    {
        var product = await productRepository.FindByIdAsync(command.Id);
        if (product == null) return null;

        product.UpdateProduct(
            command.Name,
            command.Description,
            command.PurchasePrice,
            command.SalePrice,
            command.InternalNotes,
            command.CategoryId,
            command.UnitId
        );

        // Actualizar las etiquetas
        // Primero removemos todas las etiquetas existentes
        var existingTags = product.ProductTags.ToList();
        foreach (var existingTag in existingTags)
        {
            product.RemoveTag(existingTag.TagId);
        }

        // Luego agregamos las nuevas etiquetas
        if (command.TagIds.Any())
        {
            var tags = await tagRepository.FindTagsByIdsAsync(command.TagIds);
            foreach (var tag in tags)
            {
                product.AddTag(tag.Id);
            }
        }

        try
        {
            productRepository.Update(product);
            await unitOfWork.CompleteAsync();
            return product;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> Handle(DeleteProductCommand command)
    {
        var product = await productRepository.FindByIdAsync(command.Id);
        if (product == null) return false;

        try
        {
            productRepository.Remove(product);
            await unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
} 