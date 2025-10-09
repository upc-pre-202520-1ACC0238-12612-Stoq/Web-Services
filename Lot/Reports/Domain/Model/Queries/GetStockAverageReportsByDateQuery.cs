namespace Lot.Reports.Domain.Model.Queries;

public class GetStockAverageReportsByDateQuery
{
    public DateTime FechaConsulta { get; set; }

    public GetStockAverageReportsByDateQuery(DateTime fechaConsulta)
    {
        FechaConsulta = fechaConsulta;
    }
}