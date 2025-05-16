using Markdig;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using PerfectedCheck.Data;
using PerfectedCheck.Models;
using PerfectedCheck.Services;

namespace PerfectedCheck.Controllers
{
    public class NoteController : Controller
    {
        
        private readonly UserService _userManager;
        private readonly NoteService _noteService;

        public NoteController(UserService userManager, NoteService noteService = null)
        {
            
            _userManager = userManager;
            _noteService = noteService;
        }

        [HttpGet]
        public IActionResult ViewNote(int id)
        {
            //Pull note from note id
            NoteModel note = _noteService.GetNoteFromId(id);
            
            if(note == null) return NotFound();
            
            //Convert Note content Markdown to HTML
            var pipeline = new MarkdownPipelineBuilder().Build();
            var htmlContent = Markdown.ToHtml(note.Content, pipeline);

            ViewBag.CanEdit = note.Owner.Id == _userManager.GetMyLoggedUser().Id;
            ViewBag.HtmlContent = htmlContent;
            

            return View(note);
        }

        
        public async Task<IActionResult> BrowseNotes()
        {
            var notes = _noteService.GetAllNotesOfOwner(_userManager.GetMyLoggedUser().Id);

            return View(notes);
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NoteModel model)
        {
            int newId = _noteService.Create(model);
            return RedirectToAction("ViewNote", new { id = newId });
            
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(NoteModel model)
        {
            _noteService.Update(model);

            return RedirectToAction("ViewNote", new { id = model.Id });

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            //Forbid viewing of the edit page if user not logged in
            if (!_userManager.IsLoggedIn())
            {
                return Forbid();

            }

            //Load note model into view
            var note = _noteService.GetNoteFromId(id);
            if (note == null) return NotFound();
            return View(note);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            _noteService.Delete(id);
            return RedirectToAction("Index", "Home");
        }



    }
}
