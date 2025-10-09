namespace Lot.ProductManagement.Domain.Model.Aggregates;

/// <summary>
/// Product Tag Entity
/// </summary>
/// <remarks>
/// Esta clase representa la entidad de relación entre Producto y Etiqueta.
/// Permite establecer una relación many-to-many entre productos y etiquetas.
/// </remarks>
public partial class ProductTag
{
    public int Id { get; }
    public int ProductId { get; private set; }
    public int TagId { get; private set; }

    // Navegación
    public virtual Product? Product { get; private set; }
    public virtual Tag? Tag { get; private set; }

    public ProductTag()
    {
        ProductId = 0;
        TagId = 0;
    }

    public ProductTag(int productId, int tagId)
    {
        ProductId = productId;
        TagId = tagId;
    }
} 