// *

using Lot.Reports.Domain.Model.Aggregates;
using Lot.Reports.Domain.Model.Commands;
using Lot.Reports.Domain.Repositories;
using Lot.Reports.Domain.Services;
using Lot.Shared.Domain.Repositories;

namespace Lot.Reports.Application.Internal.CommandServices;

/// <summary>
/// Command service for Category Reports
/// </summary>
public class CategoryReportCommandService(
    ICategoryReportRepository repository,
    IUnitOfWork unitOfWork) : ICategoryReportCommandService
{
    public async Task<CategoryReport?> Handle(CreateCategoryReportCommand command)
    {
        var report = new CategoryReport(
            command.Categoria,
            command.Producto,
            command.FechaConsulta,
            command.PrecioUnitario,
            command.Cantidad
        );

        try
        {
            await repository.AddAsync(report);
            await unitOfWork.CompleteAsync();
            return report;
        }
        catch (Exception)
        {
            return null;
        }
    }
}