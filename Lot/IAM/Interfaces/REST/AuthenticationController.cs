using Lot.IAM.Domain.Services;
using Lot.IAM.Interfaces.REST.Resources;
using Lot.IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Lot.IAM.Infrastructure.Authorization;

namespace Lot.IAM.Interfaces.REST
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthenticationController(IUserCommandService userCommandService) : ControllerBase
    {
        [HttpPost("sign-up")]
        [SwaggerOperation(
            Summary = "Register a new user account",
            Description =
                "Creates a new user account using provided registration details like username, password, and additional required information.",
            OperationId = "SignUp"
        )]
        [SwaggerResponse(201, "User account created successfully", typeof(UserResource))]
        [SwaggerResponse(400, "Invalid registration details provided.")]
        [SwaggerResponse(500, "Unexpected error while creating user account.")]
        [AllowAnonymous]
        public async Task<ActionResult> SignUp([FromBody] SignUpResource resource)
        {
            var signUpCommand = SignUpCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await userCommandService.Handle(signUpCommand);

            if (result is null) return BadRequest("Failed to create user account. Verify your details and try again.");

            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(result);

            return CreatedAtAction(nameof(SignUp), userResource);
        }

        [HttpPost("sign-in")]
        [SwaggerOperation(
            Summary = "Authenticate and sign in user",
            Description =
                "Authenticates the user with provided credentials, returning a token and user information upon successful login.",
            OperationId = "SignIn"
        )]
        [SwaggerResponse(201, "User authenticated successfully", typeof(AuthenticatedUserResource))]
        [SwaggerResponse(400, "Invalid login credentials.")]
        [SwaggerResponse(500, "Unexpected error during authentication.")]
        [AllowAnonymous]
        public async Task<ActionResult> SignIn([FromBody] SignInResource resource)
        {
            Console.WriteLine("🔑 Intento de inicio de sesión:");
            Console.WriteLine($"   Email: {resource.Email}");

            var signInCommand = SignInCommandFromResourceAssembler.ToCommandFromResource(resource);
            var authenticatedUser = await userCommandService.Handle(signInCommand);
            
            if (authenticatedUser.user is null)
            {
                Console.WriteLine("❌ Fallo en la autenticación");
                return BadRequest("Failed to authenticate user.");
            }

            Console.WriteLine("✅ Usuario autenticado exitosamente:");
            Console.WriteLine($"   ID: {authenticatedUser.user.Id}");
            Console.WriteLine($"   Nombre: {authenticatedUser.user.Name}");
            Console.WriteLine($"   Rol: {authenticatedUser.user.Role}");
            
            var authenticatedUserResource = AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(authenticatedUser.user, authenticatedUser.token);
            return Ok(authenticatedUserResource);
        }

        [HttpPut("change-role")]
        [SwaggerOperation(
            Summary = "Change user role",
            Description = "Changes the role of a user between Employee and Administrator.",
            OperationId = "ChangeUserRole"
        )]
        [SwaggerResponse(200, "User role changed successfully", typeof(UserResource))]
        [SwaggerResponse(400, "Invalid request or user not found.")]
        [SwaggerResponse(500, "Unexpected error while changing user role.")]
        [AuthorizeRoles("Administrator")]
        public async Task<ActionResult> ChangeUserRole([FromBody] ChangeUserRoleResource resource)
        {
            var changeRoleCommand = ChangeUserRoleCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await userCommandService.Handle(changeRoleCommand);

            if (result is null) return BadRequest("Failed to change user role. User not found.");

            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(result);
            return Ok(userResource);
        }
    }
}

