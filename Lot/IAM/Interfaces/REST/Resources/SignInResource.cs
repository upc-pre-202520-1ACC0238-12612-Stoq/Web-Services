using System.ComponentModel.DataAnnotations;

namespace Lot.IAM.Interfaces.REST.Resources
{
    /// <summary>
    ///    Represents the data provided by the client to sign in.
    /// </summary>
    /// <param name="Email">The username</param>
    /// <param name="Password">The password</param>
    public record SignInResource(
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        string Email,

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
        string Password
    );
}

