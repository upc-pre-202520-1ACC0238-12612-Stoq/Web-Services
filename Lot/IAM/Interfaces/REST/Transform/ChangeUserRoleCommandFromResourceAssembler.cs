using Lot.IAM.Domain.Model.Aggregates;
using Lot.IAM.Domain.Model.Commands;
using Lot.IAM.Interfaces.REST.Resources;

namespace Lot.IAM.Interfaces.REST.Transform
{
    public class ChangeUserRoleCommandFromResourceAssembler
    {
        public static ChangeUserRoleCommand ToCommandFromResource(ChangeUserRoleResource resource)
        {
            var newRole = Enum.Parse<UserRole>(resource.NewRole);
            return new ChangeUserRoleCommand(resource.UserId, newRole);
        }
    }
} 