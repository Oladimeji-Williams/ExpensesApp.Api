namespace ExpensesApp.API.Dtos
{
    /// <summary>
    /// Data Transfer Object for registering a new user.
    /// </summary>
    public class RegisterUserDto
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
