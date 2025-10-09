//*
using Lot.Reports.Domain.Model.Commands;
using Lot.Reports.Domain.Model.Aggregates;

namespace Lot.Reports.Domain.Services;

public interface ICategoryReportCommandService
{
    Task<CategoryReport> Handle(CreateCategoryReportCommand command);
}
