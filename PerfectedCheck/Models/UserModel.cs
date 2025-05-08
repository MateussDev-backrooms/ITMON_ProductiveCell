using System.ComponentModel.DataAnnotations;

namespace PerfectedCheck.Models
{
    public class UserModel
    {
        [Key]
        public int UID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
