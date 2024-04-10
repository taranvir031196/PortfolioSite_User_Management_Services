using System.ComponentModel.DataAnnotations;

namespace Portfolio_Site_UserManagement_Services.Dtos
{
    public class DeleteRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

    }
}
