using Microsoft.AspNetCore.Mvc;
using PerfectedCheck.Models;
using PerfectedCheck.Services;

namespace PerfectedCheck.Controllers
{
    public class TaskController : Controller
    {
        private readonly UserService _userService;
        private readonly TaskService _taskService;

        public TaskController(UserService userService, TaskService taskService)
        {
            _userService = userService;
            _taskService = taskService;
        }

        public IActionResult Index()
        {
            if (!_userService.IsLoggedIn()) return Forbid();
            int uid = _userService.GetMyLoggedUser().Id;
            var tasks = _taskService.GetAllUserTasks(uid);
            return View(tasks);
        }

        [HttpPost]
        public IActionResult Create(string task)
        {
            if (!string.IsNullOrWhiteSpace(task))
                _taskService.Create(new TaskModel { Task = task, Id = "" });

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ToggleStatus(string id)
        {
            var existing = _taskService.GetById(id);
            _taskService.SetTaskStatus(id, !existing.IsCompleted);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditView(string id)
        {
            ViewData["EditTaskId"] = id;
            int userId = _userService.GetMyLoggedUser().Id;
            var tasks = _taskService.GetAllUserTasks(userId);
            return View("Index", tasks);
        }

        [HttpPost]
        public IActionResult Edit(string id, string task, bool deleteOnEmpty = false)
        {
            if (string.IsNullOrWhiteSpace(task) || deleteOnEmpty)
            {
                _taskService.Delete(id);
            }
            else
            {
                _taskService.Update(new TaskModel { Id = id, Task = task });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            _taskService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
