//L
namespace Lot.Inventaries.Domain.Model.Commands;

public class CreateBranchCommand
{
    public string Name { get; }
    public string Type { get; }
    public string Address { get; }
    public decimal Latitude { get; }
    public decimal Longitude { get; }
    public int StockTotal { get; }

    public CreateBranchCommand(
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
    }
}

