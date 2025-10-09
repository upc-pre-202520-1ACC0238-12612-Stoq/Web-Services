using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Linq;

namespace Lot.IAM.Infrastructure.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeRolesAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public AuthorizeRolesAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            Console.WriteLine("\n🔒 ================== INICIO DE AUTORIZACIÓN ==================");
            
            // Obtener el token del header
            var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            Console.WriteLine($"📨 Header de Autorización recibido: {authHeader ?? "No presente"}");

            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length);
                Console.WriteLine($"🎫 Token extraído: {token}");
            }
            else
            {
                Console.WriteLine("⚠️ No se encontró un token Bearer en el header");
            }

            var user = context.HttpContext.User;
            Console.WriteLine($"👤 Identidad del usuario:");
            Console.WriteLine($"   - ¿Está autenticado?: {user.Identity?.IsAuthenticated ?? false}");
            Console.WriteLine($"   - Nombre de autenticación: {user.Identity?.Name ?? "No disponible"}");
            Console.WriteLine($"   - Tipo de autenticación: {user.Identity?.AuthenticationType ?? "No disponible"}");

            if (!user.Identity.IsAuthenticated)
            {
                Console.WriteLine("❌ Usuario no autenticado - Retornando 401 Unauthorized");
                context.Result = new UnauthorizedResult();
                return;
            }

            Console.WriteLine("\n📝 CLAIMS ENCONTRADOS EN EL CONTEXTO:");
            foreach (var claim in user.Claims)
            {
                Console.WriteLine($"   - Tipo: {claim.Type}");
                Console.WriteLine($"     Valor: {claim.Value}");
                Console.WriteLine($"     Emisor: {claim.Issuer}");
            }

            // Intentamos obtener el rol de diferentes claims
            var userRole = user.FindFirst(ClaimTypes.Role)?.Value ?? 
                          user.FindFirst("role")?.Value;

            Console.WriteLine($"\n🎭 INFORMACIÓN DE ROL:");
            Console.WriteLine($"   Rol del usuario: {userRole ?? "No encontrado"}");
            Console.WriteLine($"   Roles permitidos: {string.Join(", ", _roles)}");
            
            if (string.IsNullOrEmpty(userRole))
            {
                Console.WriteLine("❌ No se encontró el rol del usuario - Retornando 403 Forbidden");
                context.Result = new ForbidResult();
                return;
            }

            var hasValidRole = _roles.Any(role => 
                role.Equals(userRole, StringComparison.OrdinalIgnoreCase));

            if (!hasValidRole)
            {
                Console.WriteLine($"❌ Rol no autorizado: {userRole} - Retornando 403 Forbidden");
                context.Result = new ForbidResult();
                return;
            }

            Console.WriteLine("✅ Autorización exitosa - Acceso permitido");
            Console.WriteLine("================== FIN DE AUTORIZACIÓN ==================\n");
        }
    }
} 