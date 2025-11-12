using Lot.IAM.Domain.Model.Aggregates;

namespace Lot.IAM.Interfaces.REST.Resources
{
    /// <summary>
    /// Resource for changing user role with integer validation
    /// </summary>
    /// <param name="UserId">Target user ID</param>
    /// <param name="NewRole">New role: 0=Employee, 1=Administrator</param>
    public record ChangeUserRoleResource(int UserId, int NewRole);
} 