using Lot.IAM.Domain.Model.Aggregates;
using Lot.IAM.Domain.Repositories;
using Lot.Shared.Infraestructure.Persistence.EFC.Configuration.Extensions;
using Lot.Shared.Infraestructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Lot.IAM.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext context) : BaseRepository<User>(context), IUserRepository
    {
        public async Task<User?> FindByEmailAsync(string email)
        {
            return await context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await context.Set<User>().AddAsync(user);
        }

        public async Task UpdateUserRoleAsync(int userId, UserRole newRole)
        {
            var user = await context.Set<User>().FindAsync(userId);
            if (user != null)
            {
                // Usar reflexión para actualizar la propiedad privada
                var roleProperty = typeof(User).GetProperty("Role", BindingFlags.Public | BindingFlags.Instance);
                roleProperty?.SetValue(user, newRole);
                context.Set<User>().Update(user);
            }
        }
    }
}

