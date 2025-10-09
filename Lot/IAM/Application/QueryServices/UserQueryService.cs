using Lot.IAM.Domain.Model.Aggregates;
using Lot.IAM.Domain.Model.Queries;
using Lot.IAM.Domain.Repositories;
using Lot.IAM.Domain.Services;

namespace AyniTech.IAM.Application.QueryServices
{
    public class UserQueryService(IUserRepository userRepository) : IUserQueryService
    {
        /**
         * <summary>
         *     Handle get user by id query
         * </summary>
         * <param name="query">The query object containing the user id to search</param>
         * <returns>The user</returns>
         */
        public async Task<User?> Handle(GetUserByIdQuery query)
        {
            return await userRepository.FindByIdAsync(query.Id);
        }
    }
}

