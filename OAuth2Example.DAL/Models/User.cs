using System.ComponentModel.DataAnnotations;

namespace OAuth2Example.DAL.Models
{
    public class User : BaseModel
    {
        [Required, EmailAddress, MaxLength(30)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Salt { get; set; }
    }
}
