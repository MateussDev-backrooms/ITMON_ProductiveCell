using Microsoft.EntityFrameworkCore;
using PerfectedCheck.Data;
using PerfectedCheck.Models;

namespace PerfectedCheck.Services
{


    public class NoteService : INoteService
    {
        private readonly ProductiveCellDBContext _context;
        private readonly LoggedUserService _loggedUserService;

        public NoteService(ProductiveCellDBContext context, LoggedUserService loggedUserService)
        {
            _context = context;
            _loggedUserService = loggedUserService;
        }

        public int Create(NoteModel model)
        {
            if (!_loggedUserService.IsLoggedIn) throw new Exception("Must be logged in to create note");

            var user = _context.Users.Find(_loggedUserService.User.Id);
            NoteModel newNote = new NoteModel
            {
                
                CreatedTime = DateTime.UtcNow,
                Owner = user,
                Title = model.Title,
                Content = model.Content,
            };
            if(newNote.Title == "")
            {
                newNote.Title = "New Note";
            }

            _context.Notes.Add(newNote);
            _context.SaveChanges();
            return newNote.Id;
        }

        public void Update(NoteModel model)
        {
            NoteModel noteFromDb = _context.Notes.Include(n => n.Owner).FirstOrDefault(n => n.Id == model.Id);
            if (noteFromDb == null) throw new Exception("Note not found");
            if (!_loggedUserService.IsLoggedIn) throw new Exception("Must be logged in to edit note");
            if (_loggedUserService.User == null) throw new Exception("Logged-in user not found");
            if (_loggedUserService.User.Id != noteFromDb.Owner.Id) throw new Exception("Cannot edit notes not owned by you");

            noteFromDb.Title = model.Title;
            noteFromDb.Content = model.Content;

            _context.SaveChanges();

        }

        public void Delete(int id)
        {
            NoteModel noteFromDb = _context.Notes.Include(n => n.Owner).FirstOrDefault(n => n.Id == id);
            if (noteFromDb == null) throw new Exception("Note not found");
            if (!_loggedUserService.IsLoggedIn) throw new Exception("Must be logged in to delete note");
            if (_loggedUserService.User == null) throw new Exception("Logged-in user not found");
            if (_loggedUserService.User.Id != noteFromDb.Owner.Id) throw new Exception("Cannot delete notes not owned by you");

            _context.Notes.Remove(noteFromDb);
            _context.SaveChanges();
        }

        public NoteModel GetNoteFromId(int id)
        {
            return _context.Notes.Include(n => n.Owner).ToList().Find(n => n.Id == id);
        }

        public List<NoteModel> GetAllNotesOfOwner(int ownerId)
        {
            //Pull notes ordered by created time
            var notes = _context.Notes
                .Include(n => n.Owner)
                .OrderByDescending(n => n.CreatedTime)
                .Where(n => n.Owner.Id == ownerId)
                .ToList();
            return notes;
        }

        private int GenerateRandomID()
        {
            Random rnd = new();
            return rnd.Next(128, 2147483647);
        }


    }
}
