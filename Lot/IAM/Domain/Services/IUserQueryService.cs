using Lot.IAM.Domain.Model.Aggregates;
using Lot.IAM.Domain.Model.Queries;

namespace Lot.IAM.Domain.Services
{
    public interface IUserQueryService
    {
    /**
        * <summary>
        *     Handle get user by id query
        * </summary>
        * <param name="query">The get user by id query</param>
        * <returns>The user if found, null otherwise</returns>
        */
    Task<User?> Handle(GetUserByIdQuery query); 
    }
}

