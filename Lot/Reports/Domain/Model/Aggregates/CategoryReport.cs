//*
namespace Lot.Reports.Domain.Model.Aggregates;

/// <summary>
/// Agregado raíz que representa un reporte por categoría.
/// Encapsula las métricas calculadas del inventario para una categoría específica.
/// </summary>
public class CategoryReport
{
    public int Id { get; private set; }
    public int CategoryId { get; private set; }
    public string CategoriaNombre { get; private set; }
    public DateTime FechaConsulta { get; private set; }
    public int TotalProductos { get; private set; }
    public int StockTotal { get; private set; }
    public decimal ValorTotalInventario { get; private set; }
    public int ProductosBajoStock { get; private set; }

    // Propiedades de solo lectura calculadas
    public decimal PrecioPromedioPorUnidad => StockTotal > 0 ? ValorTotalInventario / StockTotal : 0;
    public decimal PorcentajeProductosBajoStock => TotalProductos > 0 ? (decimal)ProductosBajoStock / TotalProductos * 100 : 0;
    public bool TieneAlertasCriticas => PorcentajeProductosBajoStock > 50;

    // Constructor por defecto para EF Core
    private CategoryReport()
    {
        CategoriaNombre = string.Empty;
    }

    /// <summary>
    /// Crea un nuevo reporte de categoría con validaciones de dominio
    /// </summary>
    /// <param name="categoryId">ID de la categoría</param>
    /// <param name="categoriaNombre">Nombre de la categoría</param>
    /// <param name="fechaConsulta">Fecha de generación del reporte</param>
    /// <param name="totalProductos">Total de productos en la categoría</param>
    /// <param name="stockTotal">Stock total acumulado</param>
    /// <param name="valorTotalInventario">Valor total del inventario</param>
    /// <param name="productosBajoStock">Cantidad de productos con stock bajo</param>
    /// <returns>Nueva instancia de CategoryReport</returns>
    /// <exception cref="ArgumentException">Lanzada cuando los parámetros son inválidos</exception>
    public static CategoryReport Create(int categoryId, string categoriaNombre, DateTime fechaConsulta,
        int totalProductos, int stockTotal, decimal valorTotalInventario, int productosBajoStock)
    {
        // Validaciones de dominio
        if (categoryId <= 0)
            throw new ArgumentException("CategoryId debe ser mayor que cero", nameof(categoryId));

        if (string.IsNullOrWhiteSpace(categoriaNombre))
            throw new ArgumentException("CategoriaNombre no puede ser nulo o vacío", nameof(categoriaNombre));

        if (fechaConsulta == default)
            throw new ArgumentException("FechaConsulta debe ser una fecha válida", nameof(fechaConsulta));

        if (totalProductos < 0)
            throw new ArgumentException("TotalProductos no puede ser negativo", nameof(totalProductos));

        if (stockTotal < 0)
            throw new ArgumentException("StockTotal no puede ser negativo", nameof(stockTotal));

        if (valorTotalInventario < 0)
            throw new ArgumentException("ValorTotalInventario no puede ser negativo", nameof(valorTotalInventario));

        if (productosBajoStock < 0)
            throw new ArgumentException("ProductosBajoStock no puede ser negativo", nameof(productosBajoStock));

        if (productosBajoStock > totalProductos)
            throw new ArgumentException("ProductosBajoStock no puede ser mayor que TotalProductos", nameof(productosBajoStock));

        return new CategoryReport(categoryId, categoriaNombre, fechaConsulta,
            totalProductos, stockTotal, valorTotalInventario, productosBajoStock);
    }

    // Constructor privado para uso del factory method
    private CategoryReport(int categoryId, string categoriaNombre, DateTime fechaConsulta,
        int totalProductos, int stockTotal, decimal valorTotalInventario, int productosBajoStock)
    {
        CategoryId = categoryId;
        CategoriaNombre = categoriaNombre.Trim();
        FechaConsulta = fechaConsulta;
        TotalProductos = totalProductos;
        StockTotal = stockTotal;
        ValorTotalInventario = valorTotalInventario;
        ProductosBajoStock = productosBajoStock;
    }

    /// <summary>
    /// Determina si el reporte indica una situación crítica de inventario
    /// </summary>
    public bool EsSituacionCritica()
    {
        return PorcentajeProductosBajoStock > 50 || (TotalProductos > 0 && StockTotal == 0);
    }

    /// <summary>
    /// Determina si el reporte indica una situación de advertencia
    /// </summary>
    public bool EsSituacionAdvertencia()
    {
        return !EsSituacionCritica() && PorcentajeProductosBajoStock > 20;
    }

    /// <summary>
    /// Obtiene el nivel de riesgo del inventario
    /// </summary>
    public string ObtenerNivelRiesgo()
    {
        if (EsSituacionCritica()) return "Crítico";
        if (EsSituacionAdvertencia()) return "Advertencia";
        return "Normal";
    }
}
