//L
using Lot.Inventaries.Domain.Model.Aggregates;
using Lot.Inventaries.Domain.Model.Commands;
using Lot.Inventaries.Domain.Repositories;
using Lot.Inventaries.Domain.Services;

using Lot.Shared.Domain.Repositories;

namespace Lot.Inventaries.Application.Internal.CommandServices;

public class InventoryByProductCommandService(
    IInventoryByProductRepository repository,
    IUnitOfWork unitOfWork) : IInventoryByProductCommandService
{
    public async Task<InventoryByProduct?> Handle(CreateInventoryByProductCommand command)
    {
        var inventory = new InventoryByProduct(
            command.Categoria,
            command.Producto,
            command.FechaEntrada,
            command.Cantidad,
            command.Precio,
            command.StockMinimo,
            command.UnidadMedida
        );

        try
        {
            await repository.AddAsync(inventory);
            await unitOfWork.CompleteAsync();
            return inventory;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await repository.FindByIdAsync(id);
        if (entity == null) return false;
        await repository.DeleteAsync(id);
        await unitOfWork.CompleteAsync();
        return true;
    }
}