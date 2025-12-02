//L
namespace Lot.Inventaries.Domain.Model.Aggregates;

public class Branch
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Type { get; private set; } // 'central', 'norte', 'sur', 'este', etc.
    public string Address { get; private set; }
    public decimal Latitude { get; private set; }
    public decimal Longitude { get; private set; }
    public int StockTotal { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public Branch() { }

    public Branch(
        string name,
        string type,
        string address,
        decimal latitude,
        decimal longitude,
        int stockTotal = 0)
    {
        Name = name;
        Type = type;
        Address = address;
        Latitude = latitude;
        Longitude = longitude;
        StockTotal = stockTotal;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    public void UpdateStock(int newStockTotal)
    {
        StockTotal = newStockTotal;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(
        string? name = null,
        string? type = null,
        string? address = null,
        decimal? latitude = null,
        decimal? longitude = null)
    {
        if (name != null) Name = name;
        if (type != null) Type = type;
        if (address != null) Address = address;
        if (latitude.HasValue) Latitude = latitude.Value;
        if (longitude.HasValue) Longitude = longitude.Value;
        UpdatedAt = DateTime.UtcNow;
    }
}

