//*
using Lot.Reports.Domain.Model.Aggregates;
using Lot.Reports.Domain.Model.Queries;
using Lot.Reports.Domain.Repositories;
using Lot.Reports.Domain.Services;

namespace Lot.Reports.Application.Internal.QueryServices;

/// <summary>
/// Query service for Stock Average Reports
/// </summary>
public class StockAverageReportQueryService(IStockAverageReportRepository repository)
    : IStockAverageReportQueryService
{
    public async Task<IEnumerable<StockAverageReport>> Handle(GetAllStockAverageReportsQuery query)
    {
        return await repository.FindAllAsync();
    }

    public async Task<IEnumerable<StockAverageReport>> Handle(GetStockAverageReportsByDateQuery query)
    {
        return await repository.FindByDateAsync(query.FechaConsulta);
    }

    public async Task<StockAverageReport?> Handle(GetStockAverageReportByIdQuery query)
    {
        return await repository.FindByIdAsync(query.Id);
    }

    
}

