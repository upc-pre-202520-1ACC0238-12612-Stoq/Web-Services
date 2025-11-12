//*
namespace Lot.Reports.Interfaces.REST.Resources;

public record StockAverageReportResource(
    int Id,
    int CategoryId,
    string CategoriaNombre,
    DateTime FechaConsulta,
    decimal StockPromedioReal,
    int StockMinimoPromedio,
    int TotalProductos,
    int ProductosBajoStock,
    decimal PorcentajeBajoStock
);