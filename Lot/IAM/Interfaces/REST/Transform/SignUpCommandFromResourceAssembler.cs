using System.Globalization;
using Lot.IAM.Domain.Model.Commands;
using Lot.IAM.Interfaces.REST.Resources;

namespace Lot.IAM.Interfaces.REST.Transform
{
    public class SignUpCommandFromResourceAssembler
    {
        public static SignUpCommand ToCommandFromResource(SignUpResource resource)
        {
            //string Name, string LastName, string Email, string Password
            return new SignUpCommand(resource.Name, resource.LastName, resource.Email,
                resource.Password);
        }
    }
}

