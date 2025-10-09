using System.Security.Claims;
using System.Text;
using Lot.IAM.Application.OutBoundServices;
using Lot.IAM.Domain.Model.Aggregates;
using Lot.IAM.Infrastructure.Tokens.JWT.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Lot.IAM.Infrastructure.Tokens.JWT.Services
{
    public class TokenService(IOptions<TokenSettings> tokenSettings) : ITokenService
    {
        private readonly TokenSettings _tokenSettings = tokenSettings.Value;

        public string GenerateToken(User user)
        {
            Console.WriteLine("🎫 Generando token para usuario:");
            Console.WriteLine($"   - ID: {user.Id}");
            Console.WriteLine($"   - Nombre: {user.Name}");
            Console.WriteLine($"   - Rol: {user.Role}");

            var secret = _tokenSettings.Secret;
            var key = Encoding.ASCII.GetBytes(secret);

            // Creamos los claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("role", user.Role.ToString()) // Mantenemos este para compatibilidad
            };

            Console.WriteLine("📜 Claims generados:");
            foreach (var claim in claims)
            {
                Console.WriteLine($"   - {claim.Type}: {claim.Value}");
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JsonWebTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            Console.WriteLine("✅ Token generado exitosamente");
            return token;
        }

        /**
         * <summary>
         *     VerifyPassword token
         * </summary>
         * <param name="token">The token to validate</param>
         * <returns>The user id if the token is valid, null otherwise</returns>
         */
        public async Task<int?> ValidateToken(string token)
        {
            Console.WriteLine("🔍 Iniciando validación de token");
            
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("❌ Token nulo o vacío");
                return null;
            }

            var tokenHandler = new JsonWebTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenSettings.Secret);
            try
            {
                Console.WriteLine("🔐 Validando token con parámetros configurados");
                var tokenValidationResult = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                });

                var jwtToken = (JsonWebToken)tokenValidationResult.SecurityToken;
                var userId = int.Parse(jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value);
                
                Console.WriteLine($"✅ Token válido para usuario ID: {userId}");
                Console.WriteLine("🔍 Claims en el token:");
                foreach (var claim in jwtToken.Claims)
                {
                    Console.WriteLine($"   - {claim.Type}: {claim.Value}");
                }
                
                return userId;
            }
            catch (Exception e)
            {
                Console.WriteLine("❌ Error validando token:");
                Console.WriteLine($"   {e.Message}");
                Console.WriteLine($"   {e.StackTrace}");
                return null;
            }
        }
    }
}

