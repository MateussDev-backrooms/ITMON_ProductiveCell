using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        
        public IActionResult BrowseNotes()
        {
            var notes = _context.Notes.ToList();

            if(_context == null) { throw new Exception("DB Context is null"); }
            return View(notes);
        }
        
    }
}
