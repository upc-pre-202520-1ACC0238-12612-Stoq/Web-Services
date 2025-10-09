namespace Lot.IAM.Interfaces.REST.Resources
{
    /// <summary>
    ///    Represents the data provided by the client to sign in.
    /// </summary>
    /// <param name="Email">The username</param>
    /// <param name="Password">The password</param>
    public record SignInResource(string Email, string Password);
}

