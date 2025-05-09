using System.ComponentModel.DataAnnotations;

namespace PerfectedCheck.Models
{
    public class NoteModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedTime { get; set; };
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public virtual UserModel Owner { get; set; }
    }
}
