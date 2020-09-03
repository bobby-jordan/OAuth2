using System;
using System.ComponentModel.DataAnnotations;

namespace OAuth2Example.DAL.Models
{
    public class AuthToken : BaseModel
    {
        [Required, MaxLength(32), MinLength(32)]
        public string Token { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime ExpiresAt { get; set; }

        [Required]
        public virtual User User { get; set; }
    }
}
