using Microsoft.AspNetCore.Mvc;
using PerfectedCheck.Data;
using PerfectedCheck.Models;

namespace PerfectedCheck.Controllers
{
    //The note controller is used to control functions related to a Singular note on a given page
    public class NoteController : Controller
    {
        private readonly ProductiveCellDBContext _context;
        public NoteController(ProductiveCellDBContext context) {
            _context = context;
        }

        [HttpGet]
        public IActionResult ViewNote(int id)
        {
            var note = _context.Notes.FirstOrDefault(x => x.Id == id);

            //TODO: Check if the note's owner is matcing the currently logged in user's ID
            //var canEdit = note.Owner.UID == 1;
            ViewBag.CanEdit = true;

            return View(note);
        }
        //public IActionResult Index()
        //{
        //    //TODO: Pull a note model from database using an ID given via a 
        //    var note = new NoteModel() { Content = "blah blah blah", Title = "New note" };
        //    return View(note);
        //}
    }
}
