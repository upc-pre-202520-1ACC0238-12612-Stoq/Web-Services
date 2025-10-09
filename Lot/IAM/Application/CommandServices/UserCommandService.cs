using Lot.IAM.Application.OutBoundServices;
using Lot.IAM.Domain.Model.Aggregates;
using Lot.IAM.Domain.Model.Commands;
using Lot.IAM.Domain.Repositories;
using Lot.IAM.Domain.Services;
using Lot.Shared.Domain.Repositories;

namespace Lot.IAM.Application.CommandServices
{
    public class UserCommandService(
        IUserRepository userRepository,
        ITokenService tokenService,
        IHashingService hashingService,
        IUnitOfWork unitOfWork) : IUserCommandService
    {
        public async Task<User?> Handle(SignUpCommand command)
        {
            var hashedCommand = command with{Password = hashingService.GenerateHash(command.Password)};
            var user = new User(hashedCommand);

            try
            {
                await userRepository.AddAsync(user);
                await unitOfWork.CompleteAsync();

            }
            catch (Exception)
            {
                return null;
            }
            return user;
        }

        public async Task<(User?, string)> Handle(SignInCommand command)
        {
            var user = await userRepository.FindByEmailAsync(command.Email);
            
            if (user == null || !hashingService.VerifyHash(command.Password, user.Password))
                throw new Exception("Invalid credentials");
            
            var token = tokenService.GenerateToken(user);
            return (user, token);
        }

        public async Task<User?> Handle(ChangeUserRoleCommand command)
        {
            var user = await userRepository.FindByIdAsync(command.UserId);
            
            if (user == null)
                throw new Exception("User not found");
            
            // Aquí necesitarías actualizar el rol del usuario
            // Por ahora, como las propiedades son privadas, necesitamos un método en el repositorio
            await userRepository.UpdateUserRoleAsync(command.UserId, command.NewRole);
            await unitOfWork.CompleteAsync();
            
            return await userRepository.FindByIdAsync(command.UserId);
        }
    }
}

