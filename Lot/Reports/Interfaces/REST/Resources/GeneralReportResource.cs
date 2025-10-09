namespace Lot.Reports.Interfaces.REST.Resources;

public class GeneralReportResource
{
    public List<CategoryReportResource> CategoryReports { get; set; } = [];
    public List<StockAverageReportResource> StockAverageReports { get; set; } = [];
}