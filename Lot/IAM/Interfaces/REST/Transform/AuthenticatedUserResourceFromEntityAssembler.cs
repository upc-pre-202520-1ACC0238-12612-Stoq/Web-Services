using Lot.IAM.Domain.Model.Aggregates;
using Lot.IAM.Interfaces.REST.Resources;

namespace Lot.IAM.Interfaces.REST.Transform
{
    public class AuthenticatedUserResourceFromEntityAssembler
    {
        public static AuthenticatedUserResource ToResourceFromEntity(User user, string token)
        {
            return new AuthenticatedUserResource(user.Id, user.Name, user.LastName, token, user.Role.ToString());
        }

    }
}

