namespace ExpensesApp.API.Models.Base
{
    /// <summary>
    /// A base class for all entities with common properties.
    /// </summary>
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// The UTC date and time when the entity was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The UTC date and time when the entity was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
