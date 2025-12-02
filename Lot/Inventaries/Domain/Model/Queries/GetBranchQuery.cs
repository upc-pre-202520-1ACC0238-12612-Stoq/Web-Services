//L
namespace Lot.Inventaries.Domain.Model.Queries;

public class GetBranchQuery
{
    public int? Id { get; }
    public string? Type { get; }

    public GetBranchQuery(int? id = null, string? type = null)
    {
        Id = id;
        Type = type;
    }
}

