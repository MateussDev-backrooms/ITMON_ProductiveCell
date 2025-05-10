using Markdig;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
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
            //Pull note from note id. Return 404 page when not found
            var note = await _context.Notes.Include(n => n.Owner).FirstOrDefaultAsync(x => x.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                //Get user and check if they are the owner of the note
                //Allow edit if that is the owner
                ViewBag.CanEdit = note.Owner.Id == user?.Id;
            } else
            {
                ViewBag.CanEdit = false;
            }

            ViewBag.Username = note.Owner.UserName;
            //Convert Note content Markdown to HTML
            var pipeline = new MarkdownPipelineBuilder().Build();
            var htmlContent = Markdown.ToHtml(note.Content, pipeline);

            ViewBag.HtmlContent = htmlContent;
            

            return View(note);
        }

        
        public async Task<IActionResult> BrowseNotes()
        {
            //Return empty list if user is not logged in
            var user = await _userManager.GetUserAsync(User);
            if (user == null) { return View(new List<NoteModel>()); }

            //Pull notes ordered by created time
            var notes = _context.Notes
                .Include(n => n.Owner)
                .OrderByDescending(n => n.CreatedTime)
                .Where(n => n.Owner.Id == user.Id)
                .ToList();

            return View(notes);
        }

        //Used for generating note IDs
        private int GenerateRandomID()
        {

            Random rnd = new();
            return rnd.Next(128, 2147483647);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NoteModel model)
        {
            //Forbid create requests if user is not logged in
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Forbid();

            }

            //Create model from passed one
            var new_note = new NoteModel
            {
                Id = GenerateRandomID(),
                Owner = user,
                Title = model.Title,
                Content = model.Content,
                CreatedTime = DateTime.Now,
            };

            //Set default title
            if(new_note.Title == "")
            {
                new_note.Title = "New Note";
            }

            //Save and redirect to the view page
            _context.Notes.Add(new_note);
            await _context.SaveChangesAsync();
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
            //Forbid edit requests if user is not logged in
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Forbid();

            }

            //Find note in database
            var note = await _context.Notes.FindAsync(model.Id);
            if (note == null) return NotFound();

            //Modify the note
            note.Title = model.Title;
            note.Content = model.Content;

            //Save changes
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewNote", new { id = model.Id });

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //Forbid viewing of the edit page if user not logged in
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Forbid();

            }

            //Load note model into view
            var note = await _context.Notes.FindAsync(id);
            if (note == null) return NotFound();
            return View(note);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //Forbid not logged in users from deleting notes
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Forbid();

            //Find and remove note from database, then redirect to home
            var note = await _context.Notes.FindAsync(id);
            if (note == null) return NotFound();

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }



    }
}
