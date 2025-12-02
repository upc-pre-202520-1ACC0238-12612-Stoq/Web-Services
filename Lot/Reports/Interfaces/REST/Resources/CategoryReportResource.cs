//*
namespace Lot.Reports.Interfaces.REST.Resources;

public record CategoryReportResource(
    int Id,
    int CategoryId,
    string CategoriaNombre,
    DateTime FechaConsulta,
    int TotalProductos,
    int StockTotal,
    decimal ValorTotalInventario,
    int ProductosBajoStock
);
