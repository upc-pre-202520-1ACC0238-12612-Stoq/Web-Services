namespace Lot.Reports.Domain.Model.Aggregates;

public class StockAverageReport
{
    public int Id { get; private set; }
    public decimal StockPromedio { get; private set; }
    public string Categoria { get; private set; }
    public string Producto { get; private set; }
    public DateTime FechaConsulta { get; private set; }
    public int StockIdeal { get; private set; }
    public string Estado { get; private set; }

    public StockAverageReport(
        decimal stockPromedio,
        string categoria,
        string producto,
        DateTime fechaConsulta,
        int stockIdeal,
        string estado)
    {
        StockPromedio = stockPromedio;
        Categoria = categoria;
        Producto = producto;
        FechaConsulta = fechaConsulta;
        StockIdeal = stockIdeal;
        Estado = estado;
    }
}