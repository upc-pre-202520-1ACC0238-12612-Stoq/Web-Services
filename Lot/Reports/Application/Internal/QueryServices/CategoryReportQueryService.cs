//*
using Lot.Reports.Domain.Model.Aggregates;
using Lot.Reports.Domain.Model.Queries;
using Lot.Reports.Domain.Repositories;
using Lot.Reports.Domain.Services;

namespace Lot.Reports.Application.Internal.QueryServices;

/// <summary>
/// Query service for Category Reports
/// </summary>
public class CategoryReportQueryService(ICategoryReportRepository repository)
    : ICategoryReportQueryService
{
    public async Task<IEnumerable<CategoryReport>> Handle(GetAllCategoryReportsQuery query)
    {
        return await repository.FindAllAsync();
    }

    public async Task<IEnumerable<CategoryReport>> Handle(GetCategoryReportsByDateQuery query)
    {
        return await repository.FindByDateAsync(query.FechaConsulta);
    }
    public async Task<CategoryReport?> Handle(GetCategoryReportByIdQuery query)
    {
        return await repository.FindByIdAsync(query.Id);
    }
}
