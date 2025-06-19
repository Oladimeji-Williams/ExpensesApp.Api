namespace ExpensesApp.API.Dtos
{
    /// <summary>
    /// Data Transfer Object for creating a transaction.
    /// </summary>
    public class PostTransactionDto
    {
        public string Type { get; set; } = string.Empty;

        public double Amount { get; set; }

        public string Category { get; set; } = string.Empty;
    }
}
