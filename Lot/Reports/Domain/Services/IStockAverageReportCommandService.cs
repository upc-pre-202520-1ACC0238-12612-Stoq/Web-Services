//*
using Lot.Reports.Domain.Model.Commands;
using Lot.Reports.Domain.Model.Aggregates;

namespace Lot.Reports.Domain.Services;

public interface IStockAverageReportCommandService
{
    Task<StockAverageReport?> Handle(CreateStockAverageReportCommand command);
}