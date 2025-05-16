using Microsoft.EntityFrameworkCore;
using PerfectedCheck.Data;
using PerfectedCheck.Models;
using System.Threading.Tasks;

namespace PerfectedCheck.Services
{
    public class TaskService
    {
        private readonly ProductiveCellDBContext _context;
        private readonly LoggedUserService _loggedUserService;

        public TaskService(ProductiveCellDBContext context, LoggedUserService loggedUserService)
        {
            _context = context;
            _loggedUserService = loggedUserService;
        }

        public void Create(TaskModel task)
        {
            if (!_loggedUserService.IsLoggedIn) throw new Exception("Must be logged in to create task");
            var user = _context.Users.Find(_loggedUserService.User.Id);

            TaskModel newTask = new TaskModel
            {
                Id = GenerateTaskId(),
                Creator = user,
                CreatedAt = DateTime.UtcNow,
                Task = task.Task,
                IsCompleted = false,
            };
            _context.Tasks.Add(newTask);
            _context.SaveChanges();
        }

        public void Update(TaskModel task) 
        {
            TaskModel taskFromDB = _context.Tasks.Include(t => t.Creator).FirstOrDefault(t => t.Id == task.Id);
            if (taskFromDB == null) throw new Exception("Task not found");
            if (!_loggedUserService.IsLoggedIn) throw new Exception("Must be logged in to edit task");
            if (_loggedUserService.User == null) throw new Exception("Logged-in user not found");
            if (_loggedUserService.User.Id != taskFromDB.Creator.Id) throw new Exception("Cannot edit tasks not owned by you");

            taskFromDB.Task = task.Task;
            taskFromDB.IsCompleted = task.IsCompleted;

            _context.SaveChanges();

        }

        public TaskModel GetById(string id)
        {
            TaskModel taskFromDB = _context.Tasks.Include(t => t.Creator).FirstOrDefault(t => t.Id == id);

            return taskFromDB;
        }

        public List<TaskModel> GetAllUserTasks(int ownerId)
        {
            var tasks = _context.Tasks
                .Include(t => t.Creator)
                .OrderByDescending(t => t.CreatedAt)
                .Where(t => t.Creator.Id == ownerId)
                .ToList();

            return tasks;
        }

        public void SetTaskStatus(string id, bool status)
        {
            TaskModel taskFromDB = _context.Tasks.Include(t => t.Creator).FirstOrDefault(t => t.Id == id);
            if (taskFromDB == null) throw new Exception("Task not found");
            if (!_loggedUserService.IsLoggedIn) throw new Exception("Must be logged in to edit task");
            if (_loggedUserService.User == null) throw new Exception("Logged-in user not found");
            if (_loggedUserService.User.Id != taskFromDB.Creator.Id) throw new Exception("Cannot edit tasks not owned by you");

            taskFromDB.IsCompleted = status;
            _context.SaveChanges();
        }

        public void Delete(string id)
        {
            TaskModel taskFromDB = _context.Tasks.Include(t => t.Creator).FirstOrDefault(t => t.Id == id);
            if (taskFromDB == null) throw new Exception("Task not found");
            if (!_loggedUserService.IsLoggedIn) throw new Exception("Must be logged in to edit task");
            if (_loggedUserService.User == null) throw new Exception("Logged-in user not found");
            if (_loggedUserService.User.Id != taskFromDB.Creator.Id) throw new Exception("Cannot edit tasks not owned by you");

            _context.Tasks.Remove(taskFromDB);
            _context.SaveChanges();
        }

        private string GenerateTaskId()
        {
            string alphabet = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";
            string current = "";

            Random rng = new();
            for(int i=0; i<16; i++)
            {
                current += alphabet[rng.Next(alphabet.Length)];
            }

            return current;
        }
    }
}
