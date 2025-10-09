using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.ProductManagement.Domain.Model.Commands;
using Lot.ProductManagement.Domain.Repositories;
using Lot.ProductManagement.Domain.Services;
using Lot.Shared.Domain.Repositories;

namespace Lot.ProductManagement.Application.Internal.CommandServices;

/// <summary>
/// Combo command service implementation
/// </summary>
public class ComboCommandService(
    IComboRepository comboRepository,
    IProductRepository productRepository,
    IUnitOfWork unitOfWork) : IComboCommandService
{
    public async Task<Combo?> Handle(CreateComboCommand command)
    {
        try
        {
            var combo = new Combo(command.Name);
            
            // Primero guardamos el combo
            await comboRepository.AddAsync(combo);
            await unitOfWork.CompleteAsync();
            
            // Luego agregamos los productos si existen
            if (command.Items.Any())
            {
                foreach (var item in command.Items)
                {
                    combo.AddItem(item.ProductId, item.Quantity);
                }
                comboRepository.Update(combo);
                await unitOfWork.CompleteAsync();
            }
            
            return combo;
        }
        catch (Exception ex)
        {
            // Log the exception for debugging
            Console.WriteLine($"Error creating combo: {ex.Message}");
            return null;
        }
    }

    public async Task<Combo?> Handle(UpdateComboCommand command)
    {
        var combo = await comboRepository.FindByIdAsync(command.Id);
        if (combo == null) return null;

        combo.UpdateName(command.Name);

        // Actualizar los items
        // Primero removemos todos los items existentes
        var existingItems = combo.ComboItems.ToList();
        foreach (var existingItem in existingItems)
        {
            combo.RemoveItem(existingItem.ProductId);
        }

        // Luego agregamos los nuevos items
        if (command.Items.Any())
        {
            foreach (var item in command.Items)
            {
                combo.AddItem(item.ProductId, item.Quantity);
            }
        }

        try
        {
            comboRepository.Update(combo);
            await unitOfWork.CompleteAsync();
            return combo;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating combo: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> Handle(DeleteComboCommand command)
    {
        var combo = await comboRepository.FindByIdAsync(command.Id);
        if (combo == null) return false;

        try
        {
            comboRepository.Remove(combo);
            await unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting combo: {ex.Message}");
            return false;
        }
    }
}