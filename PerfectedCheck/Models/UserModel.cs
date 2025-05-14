using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PerfectedCheck.Models
{
    public class UserModel
    { 
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
