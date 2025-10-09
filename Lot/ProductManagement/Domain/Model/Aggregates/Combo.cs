using System.Collections.Generic;

namespace Lot.ProductManagement.Domain.Model.Aggregates;

/// <summary>
/// Combo (Kit) Aggregate Root
/// </summary>
/// <remarks>
/// Esta clase representa el agregado raíz de Combo (Kit).
/// Contiene las propiedades y métodos para gestionar la información de combos o kits de productos.
/// </remarks>
public class Combo
{
    public int Id { get; }
    public string Name { get; private set; }
    
    // Navegación
    public virtual ICollection<ComboItem> ComboItems { get; private set; }

    public Combo()
    {
        Name = string.Empty;
        ComboItems = new List<ComboItem>();
    }

    public Combo(string name)
    {
        Name = name;
        ComboItems = new List<ComboItem>();
    }

    public void AddItem(int productId, int quantity)
    {
        if (ComboItems.Any(ci => ci.ProductId == productId))
        {
            // Si el producto ya existe en el combo, actualiza la cantidad
            var existingItem = ComboItems.First(ci => ci.ProductId == productId);
            existingItem.UpdateQuantity(quantity);
            return;
        }
        
        ComboItems.Add(new ComboItem(Id, productId, quantity));
    }

    public void RemoveItem(int productId)
    {
        var comboItem = ComboItems.FirstOrDefault(ci => ci.ProductId == productId);
        if (comboItem != null)
        {
            ComboItems.Remove(comboItem);
        }
    }

    public void UpdateName(string name)
    {
        Name = name;
    }
}