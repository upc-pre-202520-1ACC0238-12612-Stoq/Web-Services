using Lot.Reports.Domain.Model.Aggregates;
using Lot.Reports.Interfaces.REST.Resources;

namespace Lot.Reports.Interfaces.REST.Transform;

public static class StockAverageReportResourceAssembler
{
    public static StockAverageReportResource ToResource(StockAverageReport report)
    {
        return new StockAverageReportResource(
            report.Id,
            report.StockPromedio,
            report.Categoria,
            report.Producto,
            report.FechaConsulta,
            report.StockIdeal,
            report.Estado
        );
    }
}