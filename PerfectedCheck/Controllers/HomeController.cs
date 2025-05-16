using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfectedCheck.Data;
using PerfectedCheck.Models;
using PerfectedCheck.Services;

namespace PerfectedCheck.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserService _userService;
        private readonly NoteService _noteService;
        


        public HomeController(UserService userService, NoteService noteService)
        {
            _noteService = noteService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            //Load note view when user logged in. Load empty view if not
            if(_userService.IsLoggedIn())
            {
                var homemodel = new HomeViewModel
                {
                    Notes = _noteService.GetAllNotesOfOwner(_userService.GetMyLoggedUser().Id)
                };
                return View(homemodel);
            } else
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
