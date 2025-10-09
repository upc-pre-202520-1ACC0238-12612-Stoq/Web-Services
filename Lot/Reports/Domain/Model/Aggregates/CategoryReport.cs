//*
namespace Lot.Reports.Domain.Model.Aggregates;

/// <summary>
/// Agregado raíz que representa un reporte por categoría.
/// </summary>
public class CategoryReport
{
    public int Id { get; private set; }
    public string Categoria { get; private set; }
    public string Producto { get; private set; }
    public DateTime FechaConsulta { get; private set; }
    public decimal PrecioUnitario { get; private set; }
    public int Cantidad { get; private set; }
    public decimal Total => PrecioUnitario * Cantidad;

    public CategoryReport()
    {
        Categoria = string.Empty;
        Producto = string.Empty;
    }

    public CategoryReport(string categoria, string producto, DateTime fechaConsulta, decimal precioUnitario, int cantidad)
    {
        Categoria = categoria;
        Producto = producto;
        FechaConsulta = fechaConsulta;
        PrecioUnitario = precioUnitario;
        Cantidad = cantidad;
    }

    public void UpdateCantidad(int nuevaCantidad)
    {
        Cantidad = nuevaCantidad;
    }

    public void UpdatePrecio(decimal nuevoPrecio)
    {
        PrecioUnitario = nuevoPrecio;
    }
}
