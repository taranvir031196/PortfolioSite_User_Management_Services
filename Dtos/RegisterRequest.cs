using System.ComponentModel.DataAnnotations;

namespace Portfolio_Site_UserManagement_Services.Dtos
{
    public class RegisterRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;

        [Required]
        public string fullName { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required, DataType(DataType.Password), Compare(nameof(Password), ErrorMessage ="Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;


    }
}
