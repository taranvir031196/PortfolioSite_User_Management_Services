using System.ComponentModel.DataAnnotations;

namespace Portfolio_Site_UserManagement_Services.Dtos
{
    public class UpdateRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string fullName { get; set; } = string.Empty;
    }
}
