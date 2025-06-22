namespace ExpensesApp.API.Dtos
{
    /// <summary>
    /// DTO for changing a user's password.
    /// </summary>
    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
