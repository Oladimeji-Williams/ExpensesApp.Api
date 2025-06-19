using ExpensesApp.API.Models.Base;

namespace ExpensesApp.API.Models
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// The user's email address.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The user's hashed password.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// The user's first name.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// The user's last name.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// The user's phone number.
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// The user's residential address.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// The user's residential address.
        /// </summary>
        public string? ProfileImagePath { get; set; }

        /// <summary>
        /// A collection of the user's transactions.
        /// </summary>
        public virtual List<Transaction> Transactions { get; set; } = new();
    }
}
