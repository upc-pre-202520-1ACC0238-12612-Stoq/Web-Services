namespace Lot.Reports.Domain.Model.Commands;
public record CreateCategoryReportCommand(
    string Categoria,
    string Producto,
    DateTime FechaConsulta,
    decimal PrecioUnitario,
    int Cantidad
);
