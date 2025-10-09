using Lot.IAM.Domain.Model.Aggregates;
using Lot.Shared.Domain.Repositories;

namespace Lot.IAM.Domain.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> FindByEmailAsync(string email); 
        Task AddAsync(User user);
        Task UpdateUserRoleAsync(int userId, UserRole newRole);
    }
}