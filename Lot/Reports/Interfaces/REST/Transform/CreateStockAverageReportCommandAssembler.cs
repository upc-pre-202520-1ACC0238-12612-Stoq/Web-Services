//*

using Lot.Reports.Domain.Model.Commands;
using Lot.Reports.Interfaces.REST.Resources;

namespace Lot.Reports.Interfaces.REST.Transform;

public static class CreateStockAverageReportCommandAssembler
{
    public static CreateStockAverageReportCommand ToCommand(CreateStockAverageReportResource resource)

    {
        return new CreateStockAverageReportCommand(
            (int)resource.StockPromedio,
            resource.Categoria,
            resource.Producto,
            resource.FechaConsulta,
            (int)resource.StockPromedio,
            resource.Estado
        );
    }
}