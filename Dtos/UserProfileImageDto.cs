// DTO class
namespace ExpensesApp.API.Dtos
{
    public class UserProfileImageDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public IFormFile? ProfileImage { get; set; }
    }
}
