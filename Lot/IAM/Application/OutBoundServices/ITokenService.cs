using Lot.IAM.Domain.Model.Aggregates;

/**
 * <summary>
 *     The token service interface
 * </summary>
 * <remarks>
 *     This interface is used to generate and validate JWT tokens
 * </remarks>
 */

namespace Lot.IAM.Application.OutBoundServices
{
    public interface ITokenService
    {
        /**
        * <summary>
        *     Generate a JWT token
        * </summary>
        * <param name="user">The user to generate the token for</param>
        * <returns>The generated token</returns>
        */
        string GenerateToken(User user);
       
        /**
         * <summary>
         *     Validate a JWT token
         * </summary>
         * <param name="token">The token to validate</param>
         * <returns>The user id if the token is valid, null otherwise</returns>
         */
        Task<int?> ValidateToken(string token);
    }
}

