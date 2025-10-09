using Lot.IAM.Application.OutBoundServices;
using BCryptNet = BCrypt.Net.BCrypt;
namespace Lot.IAM.Infrastructure.Hashing.BCrypt.Services
{
    public class HashingService : IHashingService
    {
        public string GenerateHash(string password)
        {
            return BCryptNet.HashPassword(password);
        }

        public bool VerifyHash(string password, string hash)
        {
            return BCryptNet.Verify(password, hash);
        }
    }
}

