using ExpensesApp.API.Models.Base;

namespace ExpensesApp.API.Models
{
    /// <summary>
    /// Represents a financial transaction.
    /// </summary>
    public class Transaction : BaseEntity
    {
        /// <summary>
        /// The type of the transaction (e.g., Income or Expense).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// The monetary amount of the transaction.
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// The category of the transaction (e.g., Food, Travel).
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key reference to the user.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Navigation property to the user who made the transaction.
        /// </summary>
        public virtual User? User { get; set; }
    }
}
