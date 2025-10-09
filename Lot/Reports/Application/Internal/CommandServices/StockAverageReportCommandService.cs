//*

using Lot.Reports.Domain.Model.Aggregates;
using Lot.Reports.Domain.Model.Commands;
using Lot.Reports.Domain.Repositories;
using Lot.Reports.Domain.Services;
using Lot.Shared.Domain.Repositories;

namespace Lot.Reports.Application.Internal.CommandServices;

/// <summary>
/// Command service for Stock Average Reports
/// </summary>
public class StockAverageReportCommandService(
    IStockAverageReportRepository repository,
    IUnitOfWork unitOfWork) : IStockAverageReportCommandService
{
    public async Task<StockAverageReport?> Handle(CreateStockAverageReportCommand command)
    {
        var report = new StockAverageReport(
            command.StockPromedio,
            command.Categoria,
            command.Producto,
            command.FechaConsulta,
            command.StockIdeal,
            command.Estado
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