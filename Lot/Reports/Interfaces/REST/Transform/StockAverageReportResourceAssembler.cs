using Lot.Reports.Domain.Model.Aggregates;
using Lot.Reports.Interfaces.REST.Resources;

namespace Lot.Reports.Interfaces.REST.Transform;

public static class StockAverageReportResourceAssembler
{
    public static StockAverageReportResource ToResource(StockAverageReport report)
    {
        return new StockAverageReportResource(
            report.Id,
            report.CategoryId,
            report.CategoriaNombre,
            report.FechaConsulta,
            report.StockPromedioReal,
            report.StockMinimoPromedio,
            report.TotalProductos,
            report.ProductosBajoStock,
            report.PorcentajeBajoStock
        );
    }
}