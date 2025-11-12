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
            Console.WriteLine("🔍 Buscando usuario");
            var user = await userRepository.FindByEmailAsync(command.Email);

            if (user == null)
            {
                Console.WriteLine("❌ Usuario no encontrado en la base de datos");
                throw new Exception("Invalid credentials");
            }

            Console.WriteLine("👤 Usuario encontrado, verificando contraseña...");
            bool isPasswordValid = hashingService.VerifyHash(command.Password, user.Password);

            if (!isPasswordValid)
            {
                Console.WriteLine("❌ Contraseña incorrecta");
                throw new Exception("Invalid credentials");
            }

            Console.WriteLine("✅ Contraseña verificada, generando token...");
            try
            {
                var token = tokenService.GenerateToken(user);

                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("❌ Error: Token generado es nulo o vacío");
                    throw new Exception("Token generation failed");
                }

                Console.WriteLine("🔑 Token generado correctamente");
                Console.WriteLine($"📏 Longitud del token: {token.Length} caracteres");
                Console.WriteLine($"🔍 Formato del token (primeros 50 chars): {token.Substring(0, Math.Min(50, token.Length))}...");

                return (user, token);
            }
            catch (Exception tokenEx)
            {
                Console.WriteLine($"❌ Error generando token: {tokenEx.Message}");
                throw new Exception($"Token generation failed: {tokenEx.Message}");
            }
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

