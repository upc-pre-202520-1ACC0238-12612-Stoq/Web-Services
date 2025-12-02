//*

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
/// Command service for Stock Average Reports
/// </summary>
public class StockAverageReportCommandService(
    IStockAverageReportRepository repository,
    IUnitOfWork unitOfWork,
    AppDbContext context) : IStockAverageReportCommandService
{
    public async Task<StockAverageReport?> Handle(CreateStockAverageReportCommand command)
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
        var stockPromedioReal = (decimal)inventoryData.Average(i => i.Cantidad);
        var stockMinimoPromedio = (int)inventoryData.Average(i => i.StockMinimo);
        var productosBajoStock = inventoryData.Count(i => i.StockBajo);
        var porcentajeBajoStock = totalProductos > 0
            ? (decimal)productosBajoStock / totalProductos * 100
            : 0;

        try
        {
            // Usar factory method del dominio con validaciones
            var report = StockAverageReport.Create(
                command.CategoryId,
                category.Name,
                DateTime.UtcNow,
                stockPromedioReal,
                (int)stockMinimoPromedio,
                totalProductos,
                productosBajoStock,
                porcentajeBajoStock
            );

            await repository.AddAsync(report);
            await unitOfWork.CompleteAsync();
            return report;
        }
        catch (ArgumentException ex)
        {
            // Errores de validación del dominio
            Console.WriteLine($"Error de validación en StockAverageReport: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            // Otros errores
            Console.WriteLine($"Error al crear StockAverageReport: {ex.Message}");
            return null;
        }
    }
}