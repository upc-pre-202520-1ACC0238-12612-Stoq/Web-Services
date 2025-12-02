using Lot.Sales.Domain.Model.Aggregates;
using Lot.Sales.Domain.Model.Commands;
using Lot.Sales.Domain.Repositories;
using Lot.Sales.Domain.Services;
using Lot.Shared.Domain.Repositories;
using Lot.Shared.Infraestructure.Persistence.EFC.Configuration.Extensions;
using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.Inventaries.Domain.Model.Aggregates;
using Lot.Inventaries.Domain.Repositories;
using Lot.Reports.Domain.Services;
using Lot.Reports.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Lot.Sales.Application.Internal.CommandServices;

/// <summary>
/// Command service para ventas con integración automática de inventario y reportes.
/// Maneja la creación de ventas, reducción de stock y generación de reportes.
/// </summary>
public class SaleCommandService(
    ISaleRepository saleRepository,
    IInventoryByProductRepository inventoryRepository,
    IUnitOfWork unitOfWork,
    AppDbContext context,
    ICategoryReportCommandService categoryReportService,
    IStockAverageReportCommandService stockReportService) : ISaleCommandService
{
    private readonly ICategoryReportCommandService _categoryReportService = categoryReportService;
    private readonly IStockAverageReportCommandService _stockReportService = stockReportService;

    public async Task<Sale?> Handle(CreateSaleCommand command)
    {
        // ⭐ NUEVA LÓGICA: Detectar si es venta de combo
        if (command.ComboId.HasValue)
        {
            return await HandleComboSale(command);
        }

        // Lógica existente para ventas normales (sin cambios)
        return await HandleRegularSale(command);
    }

    /// <summary>
    /// ⭐ NUEVO: Maneja ventas de combos con validación y reducción de stock múltiple
    /// </summary>
    private async Task<Sale?> HandleComboSale(CreateSaleCommand command)
    {
        // 1. Obtener combo con sus productos
        var combo = await context.Set<Combo>()
            .Include(c => c.ComboItems)
            .ThenInclude(ci => ci.Product)
            .ThenInclude(p => p.Category)
            .FirstOrDefaultAsync(c => c.Id == command.ComboId);

        if (combo == null)
        {
            Console.WriteLine($"Combo con ID {command.ComboId} no encontrado");
            return null;
        }

        // 2. Validar stock disponible para TODOS los productos del combo
        var validationResults = new List<(int ProductId, string ProductName, int Available, int Required)>();

        foreach (var comboItem in combo.ComboItems)
        {
            var inventory = await context.Set<InventoryByProduct>()
                .FirstOrDefaultAsync(i => i.ProductoId == comboItem.ProductId);

            if (inventory == null || inventory.Cantidad < (comboItem.Quantity * command.Quantity))
            {
                validationResults.Add((
                    comboItem.ProductId,
                    comboItem.Product?.Name ?? "Producto",
                    inventory?.Cantidad ?? 0,
                    comboItem.Quantity * command.Quantity
                ));
            }
        }

        if (validationResults.Any())
        {
            var errorDetails = string.Join(", ",
                validationResults.Select(v => $"{v.ProductName} (Disponible: {v.Available}, Requerido: {v.Required})"));
            Console.WriteLine($"❌ Stock insuficiente en combo '{combo.Name}': {errorDetails}");
            return null;
        }

        // 3. Reducir stock de CADA producto del combo
        var updatedInventories = new List<InventoryByProduct>();

        foreach (var comboItem in combo.ComboItems)
        {
            var inventory = await context.Set<InventoryByProduct>()
                .FirstOrDefaultAsync(i => i.ProductoId == comboItem.ProductId);

            var quantityToReduce = comboItem.Quantity * command.Quantity;
            inventory.ReduceStock(quantityToReduce);
            await inventoryRepository.UpdateAsync(inventory);
            updatedInventories.Add(inventory);

            Console.WriteLine($"📉 Stock reducido: {comboItem.Product?.Name} -{quantityToReduce} (Total: {inventory.Cantidad})");
        }

        try
        {
            // 4. Crear venta principal del combo (usando el primer producto como referencia)
            var firstItem = combo.ComboItems.First();
            var sale = Sale.CreateComboSale(
                comboId: combo.Id,
                comboName: combo.Name,
                productId: firstItem.ProductId,
                productName: combo.Name, // Nombre del combo como productName
                categoryName: "COMBO",
                quantity: command.Quantity,
                unitPrice: command.UnitPrice,
                customerName: command.CustomerName,
                notes: command.Notes
            );

            // 5. Guardar cambios en transacción
            await saleRepository.AddAsync(sale);
            await unitOfWork.CompleteAsync();

            Console.WriteLine($"✅ Venta de combo creada: {command.Quantity}x {combo.Name} para {command.CustomerName}");
            Console.WriteLine($"💰 Total venta: ${sale.TotalAmount}");
            Console.WriteLine($"📦 Stock reducido en {combo.ComboItems.Count} productos del combo");

            // 6. Generar reporte automático del combo
            await GenerateComboSaleReport(sale, combo, updatedInventories);

            return sale;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"❌ Error de validación en venta de combo: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error al crear venta de combo: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Lógica existente para ventas normales (sin cambios)
    /// </summary>
    private async Task<Sale?> HandleRegularSale(CreateSaleCommand command)
    {
        // Obtener producto para validación
        var product = await context.Set<Product>()
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == command.ProductId);

        if (product == null)
        {
            Console.WriteLine($"Producto con ID {command.ProductId} no encontrado");
            return null;
        }

        // Obtener inventario actual del producto
        var inventory = await context.Set<InventoryByProduct>()
            .FirstOrDefaultAsync(i => i.ProductoId == command.ProductId);

        if (inventory == null)
        {
            Console.WriteLine($"No existe inventario para el producto {product.Name}");
            return null;
        }

        // Validar stock disponible
        if (inventory.Cantidad < command.Quantity)
        {
            Console.WriteLine($"Stock insuficiente. Disponible: {inventory.Cantidad}, Solicitado: {command.Quantity}");
            return null;
        }

        try
        {
            // Crear la venta usando factory method
            var sale = Sale.Create(
                command.ProductId,
                product.Name,
                product.Category?.Name ?? "Sin Categoría",
                command.Quantity,
                command.UnitPrice,
                command.CustomerName,
                command.Notes
            );

            // Reducir stock automáticamente
            inventory.ReduceStock(sale.Quantity.Value);

            // Guardar cambios en transacción
            await saleRepository.AddAsync(sale);
            await inventoryRepository.UpdateAsync(inventory);
            await unitOfWork.CompleteAsync();

            Console.WriteLine($"✅ Venta creada: {sale.Quantity.Value}x {product.Name} para {command.CustomerName}");
            Console.WriteLine($"📉 Stock reducido: {inventory.Cantidad + sale.Quantity.Value} → {inventory.Cantidad}");

            // Generar reporte automático
            await GenerateAutomaticSaleReport(sale);

            return sale;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error de validación en Sale: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear venta: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sale = await saleRepository.FindByIdAsync(id);
        if (sale == null) return false;

        // Aquí podrías restaurar el stock si es necesario
        await saleRepository.DeleteAsync(sale);
        await unitOfWork.CompleteAsync();
        return true;
    }

    /// <summary>
    /// Genera reporte automático real cuando se realiza una venta
    /// Crea reportes de categoría y de stock promedio
    /// </summary>
    private async Task GenerateAutomaticSaleReport(Sale sale)
    {
        try
        {
            Console.WriteLine($"📊 Generando reportes automáticos para venta ID {sale.Id}: {sale.ProductName} - ${sale.TotalAmount}");

            // 1. Obtener información del producto vendido
            var product = await context.Set<Product>()
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == sale.ProductId);

            if (product == null)
            {
                Console.WriteLine("⚠️ No se pudo generar reporte: Producto no encontrado");
                return;
            }

            // 2. Generar Reporte de Categoría
            await GenerateCategorySaleReport(sale, product);

            // 3. Generar Reporte de Stock Promedio
            await GenerateStockSaleReport(sale, product);

            Console.WriteLine($"✅ Reportes automáticos generados exitosamente para venta {sale.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Error generando reportes automáticos: {ex.Message}");
            Console.WriteLine($"🔍 Stack trace: {ex.StackTrace}");
        }
    }

    /// <summary>
    /// Genera reporte de categoría basado en la venta
    /// </summary>
    private async Task GenerateCategorySaleReport(Sale sale, Product product)
    {
        try
        {
            // Obtener estadísticas de la categoría después de la venta
            var categoryStats = await GetCategoryStatistics(product.CategoryId);

            var categoryReport = CategoryReport.Create(
                categoryId: product.CategoryId,
                categoriaNombre: product.Category?.Name ?? "Sin Categoría",
                fechaConsulta: DateTime.Now,
                totalProductos: categoryStats.TotalProducts,
                stockTotal: categoryStats.TotalStock,
                valorTotalInventario: categoryStats.TotalValue,
                productosBajoStock: categoryStats.LowStockProducts
            );

            var result = await _categoryReportService.Handle(
                new Lot.Reports.Domain.Model.Commands.CreateCategoryReportCommand(
                    product.CategoryId
                )
            );

            if (result != null)
            {
                Console.WriteLine($"📈 Reporte de categoría '{product.Category?.Name}' generado - ID: {result.Id}");
                Console.WriteLine($"   Nivel de riesgo: {result.ObtenerNivelRiesgo()}");
                Console.WriteLine($"   Productos bajo stock: {result.ProductosBajoStock}/{result.TotalProductos}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Error generando reporte de categoría: {ex.Message}");
        }
    }

    /// <summary>
    /// Genera reporte de stock promedio basado en la venta
    /// </summary>
    private async Task GenerateStockSaleReport(Sale sale, Product product)
    {
        try
        {
            // Obtener estadísticas de stock del producto
            var stockStats = await GetProductStockStatistics(product.Id);

            var stockReport = await _stockReportService.Handle(
                new Lot.Reports.Domain.Model.Commands.CreateStockAverageReportCommand(
                    product.CategoryId
                )
            );

            if (stockReport != null)
            {
                Console.WriteLine($"📊 Reporte de stock para '{product.Name}' generado - ID: {stockReport.Id}");
                Console.WriteLine($"   Stock promedio: {stockReport.StockPromedioReal:F2}");
                Console.WriteLine($"   Estado: {stockReport.ObtenerSaludInventario()}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Error generando reporte de stock: {ex.Message}");
        }
    }

    /// <summary>
    /// Obtiene estadísticas actuales de una categoría
    /// </summary>
    private async Task<(int TotalProducts, int TotalStock, decimal TotalValue, int LowStockProducts)> GetCategoryStatistics(int categoryId)
    {
        var products = await context.Set<Product>()
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();

        var productIds = products.Select(p => p.Id).ToList();
        var inventories = await context.Set<InventoryByProduct>()
            .Where(i => productIds.Contains(i.ProductoId))
            .ToListAsync();

        var totalStock = inventories.Sum(i => i.Cantidad);
        var totalValue = inventories.Sum(i => i.Cantidad * i.Precio);
        var lowStockProducts = inventories.Count(i => i.Cantidad <= i.StockMinimo);

        return (products.Count, totalStock, totalValue, lowStockProducts);
    }

    /// <summary>
    /// Obtiene estadísticas de stock de un producto específico
    /// </summary>
    private async Task<(decimal AverageStock, decimal AverageMinStock, bool IsLowStock)> GetProductStockStatistics(int productId)
    {
        var inventory = await context.Set<InventoryByProduct>()
            .FirstOrDefaultAsync(i => i.ProductoId == productId);

        if (inventory == null)
            return (0, 0, true);

        return (inventory.Cantidad, inventory.StockMinimo, inventory.Cantidad <= inventory.StockMinimo);
    }

    /// <summary>
    /// ⭐ NUEVO: Genera reporte automático cuando se realiza una venta de combo
    /// </summary>
    private async Task GenerateComboSaleReport(Sale sale, Combo combo, List<InventoryByProduct> updatedInventories)
    {
        try
        {
            Console.WriteLine($"📊 ===== REPORTE DE VENTA DE COMBO =====");
            Console.WriteLine($"🆔 ID Venta: {sale.Id}");
            Console.WriteLine($"📦 Combo: {combo.Name}");
            Console.WriteLine($"👤 Cliente: {sale.CustomerName}");
            Console.WriteLine($"💰 Monto Total: ${sale.TotalAmount}");
            Console.WriteLine($"📅 Fecha: {sale.SaleDate:yyyy-MM-dd HH:mm:ss}");

            Console.WriteLine($"\n📋 Productos del combo vendidos:");
            foreach (var item in combo.ComboItems)
            {
                var inventory = updatedInventories.FirstOrDefault(i => i.ProductoId == item.ProductId);
                Console.WriteLine($"  • {item.Product?.Name}: {item.Quantity} unidad(es) - Stock actual: {inventory?.Cantidad ?? 0}");
            }

            Console.WriteLine($"\n📈 Impacto en inventario:");
            Console.WriteLine($"  • {combo.ComboItems.Count} productos afectados");
            Console.WriteLine($"  • Venta tipo: {sale.GetSaleType()}");

            // Aquí podrías crear un registro en ReportsModule para combos
            Console.WriteLine($"📊 Reporte de combo generado exitosamente");
            Console.WriteLine($"=======================================");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Error generando reporte de combo: {ex.Message}");
        }
    }
}