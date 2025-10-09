namespace Lot.Reports.Interfaces.REST.Resources;

public record CreateStockAverageReportResource(
    decimal StockPromedio,
    string Categoria,
    string Producto,
    DateTime FechaConsulta,
    decimal StockIdeal,
    string Estado
);