namespace Lot.ProductManagement.Domain.Model.Aggregates;

/// <summary>
/// Category Aggregate Root
/// </summary>
/// <remarks>
/// Esta clase representa el agregado raíz de Categoría.
/// Contiene las propiedades y métodos para gestionar las categorías de productos.
/// </remarks>
public partial class Category
{
    public int Id { get; }
    public string Name { get; private set; }

    // Navegación
    public virtual ICollection<Product> Products { get; private set; }

    public Category()
    {
        Name = string.Empty;
        Products = new List<Product>();
    }

    public Category(string name)
    {
        Name = name;
        Products = new List<Product>();
    }

    public void UpdateName(string name)
    {
        Name = name;
    }
} 