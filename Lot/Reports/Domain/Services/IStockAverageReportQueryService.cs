//*
using Lot.Reports.Domain.Model.Aggregates;
using Lot.Reports.Domain.Model.Queries;

namespace Lot.Reports.Domain.Services;
public interface IStockAverageReportQueryService
{
    Task<IEnumerable<StockAverageReport>> Handle(GetAllStockAverageReportsQuery query);
    Task<IEnumerable<StockAverageReport>> Handle(GetStockAverageReportsByDateQuery query);
    Task<StockAverageReport?> Handle(GetStockAverageReportByIdQuery query); 

}