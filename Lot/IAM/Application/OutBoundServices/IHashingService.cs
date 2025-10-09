namespace Lot.IAM.Application.OutBoundServices
{
    public interface IHashingService
    {
        /// <summary>
        /// Generates a hash for the given input.
        /// </summary>
        /// <param name="password">The input string to hash.</param>
        /// <returns>The hashed string.</returns>
        string GenerateHash(string password);

        /// <summary>
        /// Verifies if the given input matches the provided hash.
        /// </summary>
        /// <param name="password">The input string to verify.</param>
        /// <param name="hash">The hash to compare against.</param>
        /// <returns>True if the input matches the hash, otherwise false.</returns>
        bool VerifyHash(string password, string hash); 
    }
}

