using Lot.ProductManagement.Domain.Model.Commands;

namespace Lot.ProductManagement.Domain.Model.Aggregates;

/// <summary>
/// Product Aggregate Root
/// </summary>
/// <remarks>
/// Esta clase representa el agregado raíz de Producto.
/// Contiene las propiedades y métodos para gestionar la información de productos.
/// </remarks>
public partial class Product
{
    public int Id { get; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal PurchasePrice { get; private set; }
    public decimal SalePrice { get; private set; }
    public string InternalNotes { get; private set; }
    public int CategoryId { get; private set; }
    public int UnitId { get; private set; }
    
    // Navegación
    public virtual Category? Category { get; private set; }
    public virtual Unit? Unit { get; private set; }
    public virtual ICollection<ProductTag> ProductTags { get; private set; }

    public Product()
    {
        Name = string.Empty;
        Description = string.Empty;
        InternalNotes = string.Empty;
        CategoryId = 0;
        UnitId = 0;
        ProductTags = new List<ProductTag>();
    }

    public Product(string name, string description, decimal purchasePrice, decimal salePrice, 
        string internalNotes, int categoryId, int unitId)
    {
        Name = name;
        Description = description;
        PurchasePrice = purchasePrice;
        SalePrice = salePrice;
        InternalNotes = internalNotes;
        CategoryId = categoryId;
        UnitId = unitId;
        ProductTags = new List<ProductTag>();
    }

    public Product(CreateProductCommand command)
    {
        Name = command.Name;
        Description = command.Description;
        PurchasePrice = command.PurchasePrice;
        SalePrice = command.SalePrice;
        InternalNotes = command.InternalNotes;
        CategoryId = command.CategoryId;
        UnitId = command.UnitId;
        ProductTags = new List<ProductTag>();
    }

    public void UpdateProduct(string name, string description, decimal purchasePrice, decimal salePrice, 
        string internalNotes, int categoryId, int unitId)
    {
        Name = name;
        Description = description;
        PurchasePrice = purchasePrice;
        SalePrice = salePrice;
        InternalNotes = internalNotes;
        CategoryId = categoryId;
        UnitId = unitId;
    }

    public void AddTag(int tagId)
    {
        if (ProductTags.Any(pt => pt.TagId == tagId))
            return;
        
        ProductTags.Add(new ProductTag(Id, tagId));
    }

    public void RemoveTag(int tagId)
    {
        var productTag = ProductTags.FirstOrDefault(pt => pt.TagId == tagId);
        if (productTag != null)
        {
            ProductTags.Remove(productTag);
        }
    }
} 