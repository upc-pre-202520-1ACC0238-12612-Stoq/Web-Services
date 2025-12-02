//L
using Lot.Inventaries.Domain.Model.Aggregates;
using Lot.Inventaries.Domain.Model.Commands;
using Lot.Inventaries.Domain.Repositories;
using Lot.Inventaries.Domain.Services;
using Lot.Shared.Domain.Repositories;

namespace Lot.Inventaries.Application.Internal.CommandServices;

/// <summary>
/// Command Service para InventoryByProduct.
/// Crea inventario usando foreign key (ProductoId) para conectar con Products.
/// </summary>
public class InventoryByProductCommandService(
    IInventoryByProductRepository repository,
    IUnitOfWork unitOfWork) : IInventoryByProductCommandService
{
    public async Task<InventoryByProduct?> Handle(CreateInventoryByProductCommand command)
    {
        try
        {
            // Validar que el producto exista
            if (!(repository is Lot.Inventaries.Infraestructure.Persistence.EFC.Repositories.InventoryByProductRepository efRepository))
            {
                throw new InvalidOperationException("Repository must support FK validation");
            }

            var exists = await efRepository.ProductoExistsAsync(command.ProductoId);
            if (!exists)
            {
                throw new ArgumentException($"Product with ID {command.ProductoId} does not exist");
            }

            // Crear inventario usando FK approach con fecha automática
            var inventory = new InventoryByProduct(
                command.ProductoId,
                command.Cantidad,
                command.Precio,
                command.StockMinimo
            );

            await repository.AddAsync(inventory);
            await unitOfWork.CompleteAsync();

            // Recargar con relaciones para obtener datos enriquecidos
            inventory = await efRepository.FindByIdWithRelationsAsync(inventory.Id);

            return inventory;
        }
        catch (Exception ex)
        {
            // Log error si es necesario
            Console.WriteLine($"Error creating inventory: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var entity = await repository.FindByIdAsync(id);
            if (entity == null) return false;

            await repository.DeleteAsync(id);
            await unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting inventory with ID {id}: {ex.Message}");
            return false;
        }
    }
}