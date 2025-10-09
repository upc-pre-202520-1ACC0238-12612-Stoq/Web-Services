using Lot.IAM.Domain.Model.Aggregates;

namespace Lot.IAM.Interfaces.REST.Resources
{
    public record AuthenticatedUserResource(int Id, string Name, string LastName, string Token, string Role);
}

