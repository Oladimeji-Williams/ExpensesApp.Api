namespace ExpensesApp.API.Dtos
{
    /// <summary>
    /// Data Transfer Object for user login.
    /// </summary>
    public class LoginUserDto
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
