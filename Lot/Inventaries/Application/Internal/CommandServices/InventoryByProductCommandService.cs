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

    /// <summary>
    /// Maneja la actualización de un inventario por producto existente.
    /// Sigue el patrón de BranchCommandService.Handle().
    /// </summary>
    public async Task<InventoryByProduct?> Handle(UpdateInventoryByProductCommand command)
    {
        try
        {
            // 1. Buscar entidad existente - patrón consistente
            var inventory = await repository.FindByIdAsync(command.Id);
            if (inventory == null)
                return null;

            // 2. Obtener efRepository para validaciones y carga de relaciones
            var efRepository = repository as Lot.Inventaries.Infraestructure.Persistence.EFC.Repositories.InventoryByProductRepository;
            if (efRepository == null)
            {
                throw new InvalidOperationException("Repository must support FK validation and relation loading");
            }

            // 3. Validar ProductoId si se está actualizando a un producto diferente
            if (command.ProductoId.HasValue && command.ProductoId.Value != inventory.ProductoId)
            {
                var exists = await efRepository.ProductoExistsAsync(command.ProductoId.Value);
                if (!exists)
                {
                    throw new ArgumentException($"Product with ID {command.ProductoId.Value} does not exist");
                }
            }

            // 4. Aplicar actualizaciones usando método del dominio
            inventory.Update(
                command.ProductoId,
                command.Cantidad,
                command.Precio,
                command.StockMinimo
            );

            // 5. Persistir cambios usando Unit of Work
            await repository.UpdateAsync(inventory);
            await unitOfWork.CompleteAsync();

            // 6. Recargar con relaciones para respuesta enriquecida
            inventory = await efRepository.FindByIdWithRelationsAsync(inventory.Id);

            return inventory;
        }
        catch (InvalidOperationException ex)
        {
            // Error de configuración o estado inválido
            Console.WriteLine($"Error de configuración updating inventory ID {command.Id}: {ex.Message}");
            throw; // Re-lanzar para que el controller maneje este tipo de error
        }
        catch (ArgumentException ex)
        {
            // Error de validación de datos - no debería llegar aquí si se validó en el assembler
            Console.WriteLine($"Error de validación updating inventory ID {command.Id}: {ex.Message}");
            throw; // Re-lanzar para que el controller devuelva BadRequest
        }
        catch (Exception ex)
        {
            // Error inesperado - logging detallado para depuración
            Console.WriteLine($"Error inesperado updating inventory ID {command.Id}: {ex.GetType().Name} - {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");

            // PARA PRODUCCIÓN: Configurar logging estructurado con Serilog
            // 1. Instalar paquetes: Serilog, Serilog.Extensions.Hosting, Serilog.Sinks.Console
            // 2. Configurar en Program.cs: builder.Host.UseSerilog();
            // 3. Usar logging estructurado: logger.LogError(ex, "Error inesperado actualizando inventario {InventoryId}", command.Id);
            // Ejemplo completo:
            // using Serilog;
            // Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

            return null;
        }
    }
}