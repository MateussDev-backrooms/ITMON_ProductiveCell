using System.ComponentModel.DataAnnotations;

namespace PerfectedCheck.Models
{
    public class TaskModel
    {
        [Key]
        public int Id { get; set; }

        public string Task { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public virtual UserModel Owner { get; set; }
    }
}
