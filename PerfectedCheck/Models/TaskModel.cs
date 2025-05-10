using System.ComponentModel.DataAnnotations;

namespace PerfectedCheck.Models
{
    public class TaskModel
    {
        [Key]
        public string Id { get; set; }

        public string Task { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsCompleted { get; set; }
        public virtual UserModel Creator { get; set; }
        public virtual TaskCellModel ParentCell { get; set; }
    }
}
