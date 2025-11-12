using Lot.IAM.Domain.Model.Aggregates;
using Lot.IAM.Domain.Model.Commands;
using Lot.IAM.Interfaces.REST.Resources;

namespace Lot.IAM.Interfaces.REST.Transform
{
    /// <summary>
    /// Assembler for transforming ChangeUserRoleResource to ChangeUserRoleCommand
    /// with integer validation and safe enum conversion
    /// </summary>
    public class ChangeUserRoleCommandFromResourceAssembler
    {
        /// <summary>
        /// Converts resource to command with role validation
        /// </summary>
        /// <param name="resource">Resource containing UserId and NewRole (0=Employee, 1=Administrator)</param>
        /// <returns>Valid ChangeUserRoleCommand</returns>
        /// <exception cref="ArgumentException">Thrown when role is invalid (not 0 or 1)</exception>
        public static ChangeUserRoleCommand ToCommandFromResource(ChangeUserRoleResource resource)
        {
            // Validar que el rol esté en el rango permitido
            if (resource.NewRole < 0 || resource.NewRole > 1)
            {
                throw new ArgumentException(
                    $"Invalid role value: {resource.NewRole}. " +
                    "Allowed values: 0=Employee, 1=Administrator",
                    nameof(resource.NewRole));
            }

            // Conversión segura de int a enum
            if (!Enum.IsDefined(typeof(UserRole), resource.NewRole))
            {
                throw new ArgumentException(
                    $"Role value {resource.NewRole} is not defined in UserRole enum.",
                    nameof(resource.NewRole));
            }

            var newRole = (UserRole)resource.NewRole;
            return new ChangeUserRoleCommand(resource.UserId, newRole);
        }
    }
} 