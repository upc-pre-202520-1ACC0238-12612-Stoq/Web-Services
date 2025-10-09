using Lot.IAM.Domain.Model.Aggregates;
using Lot.IAM.Domain.Model.Commands;

namespace Lot.IAM.Domain.Services
{
    public interface IUserCommandService
    {
        Task<User?> Handle(SignUpCommand command);
        Task<(User? user, string token)> Handle(SignInCommand command);
        Task<User?> Handle(ChangeUserRoleCommand command);
    }
}

