using Lot.IAM.Domain.Model.Aggregates;

namespace Lot.IAM.Domain.Model.Commands
{
    public record ChangeUserRoleCommand(int UserId, UserRole NewRole);
} 