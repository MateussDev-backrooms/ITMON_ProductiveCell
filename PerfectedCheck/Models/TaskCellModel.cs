using System.ComponentModel.DataAnnotations;

namespace PerfectedCheck.Models
{
    public class TaskCellModel
    {
        [Key]
        public int Id { get; set; }

        public string CellName { get; set; }
        public string CellDescription { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime DueTime { get; set; }

        public virtual UserModel Owner { get; set; }

        public virtual List<TaskModel> Tasks { get; set; }

    }
}
