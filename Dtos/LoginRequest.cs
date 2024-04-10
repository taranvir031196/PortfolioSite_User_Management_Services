using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Portfolio_Site_UserManagement_Services.Dtos
{
    public class LoginRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

    }
}
