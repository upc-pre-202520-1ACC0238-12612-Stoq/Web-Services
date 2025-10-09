using Lot.IAM.Domain.Model.Commands;
using Lot.IAM.Interfaces.REST.Resources;

namespace Lot.IAM.Interfaces.REST.Transform
{
    public class SignInCommandFromResourceAssembler
    {
        public static SignInCommand ToCommandFromResource(SignInResource resource)
        {
            return new SignInCommand(resource.Email, resource.Password);
        }
    }
}

