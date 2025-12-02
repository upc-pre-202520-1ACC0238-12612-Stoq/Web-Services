//L
namespace Lot.Inventaries.Interfaces.REST.Resources;

public class CreateBranchResource
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public int StockTotal { get; set; } = 0;
}

