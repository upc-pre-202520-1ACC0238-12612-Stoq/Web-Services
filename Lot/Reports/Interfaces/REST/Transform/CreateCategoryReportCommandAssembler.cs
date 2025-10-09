//*

using Lot.Reports.Domain.Model.Commands;
using Lot.Reports.Interfaces.REST.Resources;

namespace Lot.Reports.Interfaces.REST.Transform;

public static class CreateCategoryReportCommandAssembler
{
    public static CreateCategoryReportCommand ToCommand(CreateCategoryReportResource  resource)
    {
        return new CreateCategoryReportCommand(
            resource.Categoria,
            resource.Producto,
            resource.FechaConsulta,
            resource.PrecioUnitario,
            resource.Cantidad
        );
    }
}