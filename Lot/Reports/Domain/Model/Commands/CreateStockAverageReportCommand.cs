namespace Lot.Reports.Domain.Model.Commands;

public record CreateStockAverageReportCommand(
    decimal StockPromedio,
    string Categoria,
    string Producto,
    DateTime FechaConsulta,
    int StockIdeal,
    string Estado
);

