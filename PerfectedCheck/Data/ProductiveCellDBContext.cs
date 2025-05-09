using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PerfectedCheck.Models;

namespace PerfectedCheck.Data
{
    public class ProductiveCellDBContext : IdentityDbContext<UserModel>
    {

        public DbSet<NoteModel> Notes { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<TaskCellModel> TaskCells { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=productivity_cell;user=root;password=Mo2016cq?;port=3306");
        }
    }
}