using Lot.IAM.Domain.Model.Aggregates;
using Lot.IAM.Interfaces.REST.Resources;

namespace Lot.IAM.Interfaces.REST.Transform
{
    public class UserResourceFromEntityAssembler
    {
        public static UserResource ToResourceFromEntity(User user)
        {
            return new UserResource(user.Id, user.Name, user.Email);
        }
    }
}

