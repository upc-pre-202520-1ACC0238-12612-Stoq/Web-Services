namespace Lot.ProductManagement.Domain.Model.Aggregates;

/// <summary>
/// ComboItem (KitItem) Entity
/// </summary>
/// <remarks>
/// Esta clase representa un ítem dentro de un combo o kit.
/// Contiene las propiedades y métodos para gestionar la información de los productos incluidos en un combo.
/// </remarks>
public class ComboItem
{
    public int Id { get; }
    public int ComboId { get; private set; }
    public int ProductId { get; private set; }
    public int Quantity { get; private set; }
    
    // Navegación
    public virtual Combo? Combo { get; private set; }
    public virtual Product? Product { get; private set; }

    public ComboItem()
    {
        ComboId = 0;
        ProductId = 0;
        Quantity = 0;
    }

    public ComboItem(int comboId, int productId, int quantity)
    {
        ComboId = comboId;
        ProductId = productId;
        Quantity = quantity;
    }

    public void UpdateQuantity(int quantity)
    {
        Quantity = quantity;
    }
}