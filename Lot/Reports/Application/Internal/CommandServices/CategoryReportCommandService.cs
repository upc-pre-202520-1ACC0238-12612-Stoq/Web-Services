// *

using Lot.Reports.Domain.Model.Aggregates;
using Lot.Reports.Domain.Model.Commands;
using Lot.Reports.Domain.Repositories;
using Lot.Reports.Domain.Services;
using Lot.Shared.Domain.Repositories;
using Lot.Shared.Infraestructure.Persistence.EFC.Configuration.Extensions;
using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.Inventaries.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Lot.Reports.Application.Internal.CommandServices;

/// <summary>
/// Command service for Category Reports
/// </summary>
public class CategoryReportCommandService(
    ICategoryReportRepository repository,
    IUnitOfWork unitOfWork,
    AppDbContext context) : ICategoryReportCommandService
{
    public async Task<CategoryReport?> Handle(CreateCategoryReportCommand command)
    {
        // Obtener categoría
        var category = await context.Set<Category>()
            .FirstOrDefaultAsync(c => c.Id == command.CategoryId);

        if (category == null)
            return null;

        // Consultar inventario por categoría
        var inventoryData = await context.Set<InventoryByProduct>()
            .Include(i => i.Product)
            .Where(i => i.Product != null && i.Product.CategoryId == command.CategoryId)
            .ToListAsync();

        if (!inventoryData.Any())
            return null;

        // Calcular métricas reales
        var totalProductos = inventoryData.Count;
        var stockTotal = inventoryData.Sum(i => i.Cantidad);
        var valorTotalInventario = inventoryData.Sum(i => i.Cantidad * i.Precio);
        var productosBajoStock = inventoryData.Count(i => i.StockBajo);

        try
        {
            // Usar factory method del dominio con validaciones
            var report = CategoryReport.Create(
                command.CategoryId,
                category.Name,
                DateTime.UtcNow,
                totalProductos,
                stockTotal,
                valorTotalInventario,
                productosBajoStock
            );

            await repository.AddAsync(report);
            await unitOfWork.CompleteAsync();
            return report;
        }
        catch (ArgumentException ex)
        {
            // Errores de validación del dominio
            Console.WriteLine($"Error de validación en CategoryReport: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            // Otros errores
            Console.WriteLine($"Error al crear CategoryReport: {ex.Message}");
            return null;
        }
    }
}