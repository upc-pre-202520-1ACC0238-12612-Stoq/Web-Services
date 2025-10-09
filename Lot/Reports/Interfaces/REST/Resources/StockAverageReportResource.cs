//*
namespace Lot.Reports.Interfaces.REST.Resources;

public record StockAverageReportResource(
    int Id,
    decimal StockPromedio,
    string Categoria,
    string Producto,
    DateTime FechaConsulta,
    decimal StockIdeal,
    string Estado
);