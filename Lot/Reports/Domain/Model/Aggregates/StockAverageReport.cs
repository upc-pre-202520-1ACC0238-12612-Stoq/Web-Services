namespace Lot.Reports.Domain.Model.Aggregates;

/// <summary>
/// Agregado raíz que representa un reporte de stock promedio por categoría.
/// Encapsula análisis estadísticos del inventario y métricas de rendimiento.
/// </summary>
public class StockAverageReport
{
    public int Id { get; private set; }
    public int CategoryId { get; private set; }
    public string CategoriaNombre { get; private set; }
    public DateTime FechaConsulta { get; private set; }
    public decimal StockPromedioReal { get; private set; }
    public int StockMinimoPromedio { get; private set; }
    public int TotalProductos { get; private set; }
    public int ProductosBajoStock { get; private set; }
    public decimal PorcentajeBajoStock { get; private set; }

    // Propiedades calculadas de negocio
    public decimal RatioStockPromedioMinimo => StockMinimoPromedio > 0 ? StockPromedioReal / StockMinimoPromedio : 0;
    public bool TieneStockAdecuado => StockPromedioReal >= StockMinimoPromedio * 1.5m;
    public bool RequiereReabastecimientoUrgente => PorcentajeBajoStock > 60;

    // Constructor por defecto para EF Core
    private StockAverageReport()
    {
        CategoriaNombre = string.Empty;
    }

    /// <summary>
    /// Crea un nuevo reporte de stock promedio con validaciones de dominio
    /// </summary>
    /// <param name="categoryId">ID de la categoría</param>
    /// <param name="categoriaNombre">Nombre de la categoría</param>
    /// <param name="fechaConsulta">Fecha de generación del reporte</param>
    /// <param name="stockPromedioReal">Stock promedio real calculado</param>
    /// <param name="stockMinimoPromedio">Stock mínimo promedio</param>
    /// <param name="totalProductos">Total de productos analizados</param>
    /// <param name="productosBajoStock">Productos con stock bajo</param>
    /// <param name="porcentajeBajoStock">Porcentaje de productos con stock bajo</param>
    /// <returns>Nueva instancia de StockAverageReport</returns>
    /// <exception cref="ArgumentException">Lanzada cuando los parámetros son inválidos</exception>
    public static StockAverageReport Create(
        int categoryId,
        string categoriaNombre,
        DateTime fechaConsulta,
        decimal stockPromedioReal,
        int stockMinimoPromedio,
        int totalProductos,
        int productosBajoStock,
        decimal porcentajeBajoStock)
    {
        // Validaciones de dominio
        if (categoryId <= 0)
            throw new ArgumentException("CategoryId debe ser mayor que cero", nameof(categoryId));

        if (string.IsNullOrWhiteSpace(categoriaNombre))
            throw new ArgumentException("CategoriaNombre no puede ser nulo o vacío", nameof(categoriaNombre));

        if (fechaConsulta == default)
            throw new ArgumentException("FechaConsulta debe ser una fecha válida", nameof(fechaConsulta));

        if (stockPromedioReal < 0)
            throw new ArgumentException("StockPromedioReal no puede ser negativo", nameof(stockPromedioReal));

        if (stockMinimoPromedio < 0)
            throw new ArgumentException("StockMinimoPromedio no puede ser negativo", nameof(stockMinimoPromedio));

        if (totalProductos <= 0)
            throw new ArgumentException("TotalProductos debe ser mayor que cero", nameof(totalProductos));

        if (productosBajoStock < 0)
            throw new ArgumentException("ProductosBajoStock no puede ser negativo", nameof(productosBajoStock));

        if (productosBajoStock > totalProductos)
            throw new ArgumentException("ProductosBajoStock no puede ser mayor que TotalProductos", nameof(productosBajoStock));

        if (porcentajeBajoStock < 0 || porcentajeBajoStock > 100)
            throw new ArgumentException("PorcentajeBajoStock debe estar entre 0 y 100", nameof(porcentajeBajoStock));

        return new StockAverageReport(categoryId, categoriaNombre, fechaConsulta,
            stockPromedioReal, stockMinimoPromedio, totalProductos, productosBajoStock, porcentajeBajoStock);
    }

    // Constructor privado para uso del factory method
    private StockAverageReport(
        int categoryId,
        string categoriaNombre,
        DateTime fechaConsulta,
        decimal stockPromedioReal,
        int stockMinimoPromedio,
        int totalProductos,
        int productosBajoStock,
        decimal porcentajeBajoStock)
    {
        CategoryId = categoryId;
        CategoriaNombre = categoriaNombre.Trim();
        FechaConsulta = fechaConsulta;
        StockPromedioReal = stockPromedioReal;
        StockMinimoPromedio = stockMinimoPromedio;
        TotalProductos = totalProductos;
        ProductosBajoStock = productosBajoStock;
        PorcentajeBajoStock = porcentajeBajoStock;
    }

    /// <summary>
    /// Determina la salud del inventario basada en métricas de stock
    /// </summary>
    public string ObtenerSaludInventario()
    {
        if (RequiereReabastecimientoUrgente) return "Crítica";
        if (PorcentajeBajoStock > 40) return "Deficiente";
        if (PorcentajeBajoStock > 20) return "Regular";
        if (TieneStockAdecuado) return "Óptima";
        return "Aceptable";
    }

    /// <summary>
    /// Determina la prioridad de atención para esta categoría
    /// </summary>
    public string ObtenerPrioridadAtencion()
    {
        if (RequiereReabastecimientoUrgente) return "Alta";
        if (PorcentajeBajoStock > 30 || RatioStockPromedioMinimo < 1.2m) return "Media";
        return "Baja";
    }

    /// <summary>
    /// Genera recomendaciones basadas en el análisis de stock
    /// </summary>
    public List<string> GenerarRecomendaciones()
    {
        var recomendaciones = new List<string>();

        if (RequiereReabastecimientoUrgente)
        {
            recomendaciones.Add("Reabastecimiento urgente requerido - más del 60% de productos con stock bajo");
        }

        if (StockPromedioReal < StockMinimoPromedio)
        {
            recomendaciones.Add("Stock promedio inferior al mínimo recomendado");
        }

        if (RatioStockPromedioMinimo < 1.5m)
        {
            recomendaciones.Add("Considerar aumentar niveles de stock de seguridad");
        }

        if (TieneStockAdecuado)
        {
            recomendaciones.Add("Niveles de stock adecuados - mantener política actual");
        }

        if (PorcentajeBajoStock == 0)
        {
            recomendaciones.Add("Todos los productos con stock adecuado - excelente gestión");
        }

        return recomendaciones;
    }

    /// <summary>
    /// Calcula el puntaje de eficiencia del inventario (0-100)
    /// </summary>
    public int CalcularPuntajeEficiencia()
    {
        int puntaje = 100;

        // Penalización por productos con stock bajo
        puntaje -= (int)(PorcentajeBajoStock * 0.5m);

        // Penalización por stock promedio bajo
        if (StockPromedioReal < StockMinimoPromedio)
        {
            puntaje -= 30;
        }
        else if (RatioStockPromedioMinimo < 1.5m)
        {
            puntaje -= (int)((1.5m - RatioStockPromedioMinimo) * 20);
        }

        // Bonificación por stock óptimo
        if (TieneStockAdecuado && PorcentajeBajoStock < 10)
        {
            puntaje = Math.Min(100, puntaje + 10);
        }

        return Math.Max(0, Math.Min(100, puntaje));
    }
}