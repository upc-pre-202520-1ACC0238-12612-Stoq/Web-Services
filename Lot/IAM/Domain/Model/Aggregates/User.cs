using Lot.IAM.Domain.Model.Commands;
using System.ComponentModel.DataAnnotations;

namespace Lot.IAM.Domain.Model.Aggregates
{
    public enum UserRole
    {
        Employee = 0,
        Administrator = 1
    }

    public class User
    {
        public int Id { get; private set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; private set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; private set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; private set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; private set; }

        public UserRole Role { get; private set; }

        public User()
        {
            Name = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            Role = UserRole.Administrator; // Por defecto es administrador
        }

        public User(SignInCommand command)
        {
            // Validar formato de email también en sign-in para consistencia
            if (!IsValidEmail(command.Email))
                throw new ArgumentException("Invalid email format");

            Email = command.Email;
            Password = command.Password;
        }

        public User(SignUpCommand command)
        {
            // Validar formato de email antes de asignar
            if (!IsValidEmail(command.Email))
                throw new ArgumentException("Invalid email format");

            LastName = command.LastName;
            Name = command.Name;
            Email = command.Email;
            Password = command.Password;
            Role = UserRole.Administrator; // Por defecto es administrador
        }

        /// <summary>
        /// Valida el formato de un email usando el método estándar de .NET
        /// </summary>
        /// <param name="email">Email a validar</param>
        /// <returns>True si el email tiene un formato válido</returns>
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Usar la clase MailAddress para validación robusta
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        } 
    }
}

