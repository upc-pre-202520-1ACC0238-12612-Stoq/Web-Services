//L
namespace Lot.Inventaries.Domain.Model.Commands;

public class UpdateBranchCommand
{
    public int Id { get; }
    public string? Name { get; }
    public string? Type { get; }
    public string? Address { get; }
    public decimal? Latitude { get; }
    public decimal? Longitude { get; }

    public UpdateBranchCommand(
        int id,
        string? name = null,
        string? type = null,
        string? address = null,
        decimal? latitude = null,
        decimal? longitude = null)
    {
        Id = id;
        Name = name;
        Type = type;
        Address = address;
        Latitude = latitude;
        Longitude = longitude;
    }
}

