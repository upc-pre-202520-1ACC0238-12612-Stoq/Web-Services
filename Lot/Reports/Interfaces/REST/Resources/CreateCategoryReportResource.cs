namespace Lot.Reports.Interfaces.REST.Resources;

public record CreateCategoryReportResource(
    string Categoria,
    string Producto,
    DateTime FechaConsulta,
    decimal PrecioUnitario,
    int Cantidad
);