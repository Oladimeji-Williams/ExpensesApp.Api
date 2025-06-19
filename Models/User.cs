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
        /// A collection of the user's transactions.
        /// </summary>
        public virtual List<Transaction> Transactions { get; set; } = new();
    }
}
