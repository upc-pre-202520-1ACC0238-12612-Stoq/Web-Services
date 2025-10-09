namespace Lot.Reports.Domain.Model.Queries;
public class GetCategoryReportsByDateQuery
{
    public DateTime FechaConsulta { get; set; }

    public GetCategoryReportsByDateQuery(DateTime fechaConsulta)
    {
        FechaConsulta = fechaConsulta;
    }
}
