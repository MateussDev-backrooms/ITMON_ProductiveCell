using Microsoft.AspNetCore.Mvc;
using PerfectedCheck.Models;

namespace PerfectedCheck.Controllers
{
    public class NoteController : Controller
    {
        public IActionResult Index()
        {
            var note = new NoteModel() { Content = "blah blah blah", Title="New note" };
            return View(note);
        }
    }
}
