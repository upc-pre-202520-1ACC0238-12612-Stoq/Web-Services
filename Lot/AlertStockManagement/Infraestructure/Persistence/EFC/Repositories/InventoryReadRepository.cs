using Lot.AlertStockManagement.Domain.Model.Queries;
using Lot.AlertStockManagement.Domain.Model.Aggregates;
using Lot.AlertStockManagement.Domain.Repositories;
using Lot.Shared.Infraestructure.Persistence.EFC.Configuration.Extensions;
using Lot.Inventaries.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Lot.AlertStockManagement.Infraestructure.Persistence.EFC.Repositories; 
public class InventoryReadRepository : IInventoryReadRepository, IStockAlertReadRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<InventoryReadRepository> _logger;

    public InventoryReadRepository(AppDbContext context, ILogger<InventoryReadRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<StockAlertItem>> GetStockAlertsAsync(StockAlertQuery query)
    {
        try
        {
            _logger.LogInformation("Iniciando consulta de alertas de stock");

            // ✅ Validación de entrada
            if (query == null)
            {
                _logger.LogError("El parámetro query es nulo");
                throw new ArgumentNullException(nameof(query));
            }

            _logger.LogDebug("Construyendo consulta con filtros: CategoryId={CategoryId}",
                query.CategoryId);

            var queryable = _context.Set<InventoryByProduct>()
                .Include(i => i.Product)
                .ThenInclude(p => p.Category)
                .AsQueryable();

        queryable = queryable.Where(i => i.Cantidad <= i.StockMinimo);

        // Filtro por categoría ID
        if (query.CategoryId.HasValue)
        {
            queryable = queryable.Where(i => i.Product.CategoryId == query.CategoryId.Value);
        }

        // ✅ Filtro por nombre de categoría con validación y seguridad
        if (!string.IsNullOrWhiteSpace(query.CategoryName))
        {
            // ✅ Sanitización del input para prevenir SQL injection
            var sanitizedName = query.CategoryName.Trim();
            if (sanitizedName.Length > 100) // ✅ Límite de longitud
                sanitizedName = sanitizedName.Substring(0, 100);

            queryable = queryable.Where(i =>
                i.Product.Category != null &&
                i.Product.Category.Name.ToLower().Contains(sanitizedName.ToLower()));
        }

        // ✅ Filtro por fechas con validación
        if (query.FromDate.HasValue)
        {
            // ✅ Validar que la fecha no sea futura
            if (query.FromDate.Value > DateTime.Today)
                throw new ArgumentException("La fecha de inicio no puede ser futura", nameof(query.FromDate));

            queryable = queryable.Where(i => i.FechaEntrada >= query.FromDate.Value);
        }

        if (query.ToDate.HasValue)
        {
            // ✅ Validar que la fecha no sea futura
            if (query.ToDate.Value > DateTime.Today.AddDays(1))
                throw new ArgumentException("La fecha fin no puede ser futura", nameof(query.ToDate));

            queryable = queryable.Where(i => i.FechaEntrada <= query.ToDate.Value);
        }

        // ✅ Validar rango de fechas
        if (query.FromDate.HasValue && query.ToDate.HasValue)
        {
            if (query.FromDate.Value > query.ToDate.Value)
                throw new ArgumentException("La fecha de inicio no puede ser mayor que la fecha fin");
        }

        var alerts = await queryable
            .Select(i => new StockAlertItem
            {
                ProductName = i.Product != null ? i.Product.Name : "Producto desconocido",
                CategoryName = i.Product != null && i.Product.Category != null ? i.Product.Category.Name : "Sin categoría",
                Quantity = i.Cantidad,
                MinStock = i.StockMinimo,
                EntryDate = i.FechaEntrada,
                Price = i.Precio,
                StockDeficit = Math.Max(0, i.StockMinimo - i.Cantidad),
                AlertLevel = GetAlertLevel(i.Cantidad, i.StockMinimo)
            })
            .ToListAsync();

            _logger.LogInformation("Consulta completada exitosamente. Se encontraron {Count} alertas", alerts.Count);
            return alerts;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al consultar alertas de stock");
            throw new ApplicationException("Error al obtener las alertas de stock", ex);
        }
    }

    private static string GetAlertLevel(int current, int minimum)
    {
        // ✅ Manejo de casos edge y validación de entrada
        if (current < 0) return "Error - Stock negativo";
        if (minimum <= 0) return "Error - Stock mínimo inválido";
        if (current == 0) return "Crítico - Sin stock";

        // ✅ Cálculo más claro y eficiente usando decimal para precisión
        decimal fiftyPercent = minimum * 0.5m;
        if (current <= fiftyPercent) return "Alto - Stock muy bajo";
        if (current <= minimum) return "Medio - Stock bajo";
        return "Normal";
    }
}