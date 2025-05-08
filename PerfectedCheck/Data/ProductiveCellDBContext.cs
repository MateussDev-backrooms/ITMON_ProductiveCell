using Microsoft.EntityFrameworkCore;
using PerfectedCheck.Models;

namespace PerfectedCheck.Data
{
    public class ProductiveCellDBContext : DbContext
    {
        //public PerfectedCheckContext(DbContextOptions<PerfectedCheckContext> options) :base(options) { }

        public DbSet<NoteModel> Notes { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=productivity_cell;user=root;password=Mo2016cq?;port=3306");
        }
    }
}


//Make all models
// > add Migration