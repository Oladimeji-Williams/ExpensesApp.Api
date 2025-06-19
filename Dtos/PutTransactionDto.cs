namespace ExpensesApp.API.Dtos
{
    /// <summary>
    /// Data Transfer Object for updating an existing transaction.
    /// </summary>
    public class PutTransactionDto
    {
        public string Type { get; set; } = string.Empty;

        public double Amount { get; set; }

        public string Category { get; set; } = string.Empty;
    }
}
