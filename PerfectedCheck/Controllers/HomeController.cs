using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfectedCheck.Data;
using PerfectedCheck.Models;

namespace PerfectedCheck.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductiveCellDBContext _context;
        private readonly UserManager<UserModel> _userManager;

        public HomeController(ILogger<HomeController> logger, ProductiveCellDBContext context, UserManager<UserModel> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var model = new HomeViewModel
                {
                    Notes = _context.Notes
                    .Include(n => n.Owner)
                    .OrderByDescending(n => n.CreatedTime)
                    .Where(n => n.Owner.Id == user.Id)
                    .ToList()
                };
                return View(model);
            }
            else
            {
                return View(new HomeViewModel { Notes = new List<NoteModel>() });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
