using Lot.Reports.Domain.Model.Aggregates;
using Lot.Reports.Domain.Model.Queries;

namespace Lot.Reports.Domain.Services;

public interface ICategoryReportQueryService
{
    Task<IEnumerable<CategoryReport>> Handle(GetAllCategoryReportsQuery query);
    Task<IEnumerable<CategoryReport>> Handle(GetCategoryReportsByDateQuery query);
    Task<CategoryReport?> Handle(GetCategoryReportByIdQuery query); 
}