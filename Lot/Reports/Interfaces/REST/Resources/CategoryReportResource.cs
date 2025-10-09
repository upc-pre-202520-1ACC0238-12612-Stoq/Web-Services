//*
namespace Lot.Reports.Interfaces.REST.Resources;

public record CategoryReportResource(
    int Id,
    string Categoria,
    string Producto,
    DateTime FechaConsulta,
    decimal PrecioUnitario,
    int Cantidad
);
