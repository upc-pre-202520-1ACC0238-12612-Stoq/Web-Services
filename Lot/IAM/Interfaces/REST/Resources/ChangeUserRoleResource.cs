using Lot.IAM.Domain.Model.Aggregates;

namespace Lot.IAM.Interfaces.REST.Resources
{
    public record ChangeUserRoleResource(int UserId, string NewRole);
} 