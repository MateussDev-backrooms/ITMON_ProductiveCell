using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using PerfectedCheck.Data;
using PerfectedCheck.Models;

namespace PerfectedCheck.Controllers
{
    public class NoteController : Controller
    {
        private readonly ProductiveCellDBContext _context;
        private readonly UserManager<UserModel> _userManager;

        public NoteController(ProductiveCellDBContext context, UserManager<UserModel> userManager) {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> ViewNote(int id)
        {
            var note = await _context.Notes.Include(n => n.Owner).FirstOrDefaultAsync(x => x.Id == id);
            if (note == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                ViewBag.CanEdit = note.Owner.Id == user?.Id;
                ViewBag.Username = note.Owner.NormalizedUserName;
            } else
            {
                ViewBag.CanEdit = false;

            }
            

            return View(note);
        }

        
        public IActionResult BrowseNotes()
        {
            var notes = _context.Notes.ToList();

            if(_context == null) { throw new Exception("DB Context is null"); }
            return View(notes);
        }

        private int GenerateRandomID()
        {
            Random rnd = new();
            return rnd.Next(128, 2147483647);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NoteModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Forbid();

            }

            var new_note = new NoteModel
            {
                Id = GenerateRandomID(),
                Owner = user,
                Title = model.Title,
                Content = model.Content,
                CreatedTime = DateTime.Now,
            };

            if(new_note.Title == "")
            {
                new_note.Title = "New Note";
            }

            _context.Notes.Add(new_note);
            var prog = await _context.SaveChangesAsync();
            return RedirectToAction("ViewNote", new { id = new_note.Id });
            
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NoteModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Forbid();

            }

            var new_note = new NoteModel
            {
                Id = GenerateRandomID(),
                Owner = user,
                Title = model.Title,
                Content = model.Content,
                CreatedTime = DateTime.Now,
            };

            if (new_note.Title == "")
            {
                new_note.Title = "New Note";
            }

            _context.Notes.Add(new_note);
            var prog = await _context.SaveChangesAsync();
            return RedirectToAction("ViewNote", new { id = new_note.Id });

        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }



    }
}
