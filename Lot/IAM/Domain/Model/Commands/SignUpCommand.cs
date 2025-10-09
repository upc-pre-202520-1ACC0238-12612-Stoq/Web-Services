namespace Lot.IAM.Domain.Model.Commands
{
    public record SignUpCommand(string Name, string LastName, string Email, string Password);
}

