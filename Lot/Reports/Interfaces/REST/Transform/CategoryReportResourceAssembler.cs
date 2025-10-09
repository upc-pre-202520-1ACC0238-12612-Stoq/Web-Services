//*

using Lot.Reports.Domain.Model.Aggregates;
using Lot.Reports.Interfaces.REST.Resources;

namespace Lot.Reports.Interfaces.REST.Transform;

public static class CategoryReportResourceAssembler
{
    public static CategoryReportResource ToResource(CategoryReport report)
    {
        return new CategoryReportResource(
            report.Id,
            report.Categoria,
            report.Producto,
            report.FechaConsulta,
            report.PrecioUnitario,
            report.Cantidad
        );
    }
}