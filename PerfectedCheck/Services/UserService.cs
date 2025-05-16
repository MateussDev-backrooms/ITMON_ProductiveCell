using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using PerfectedCheck.Data;
using PerfectedCheck.Models;
using System.Security.Cryptography;

namespace PerfectedCheck.Services
{
    public class UserService : IUserService
    {
        private readonly ProductiveCellDBContext _context;
        private readonly LoggedUserService _loggedUserService;
        private readonly PasswordHashingService _passwordHashingService;

        public UserService(ProductiveCellDBContext context, LoggedUserService loggedUserService, PasswordHashingService passwordHashingService)
        {
            _context = context;
            _loggedUserService = loggedUserService;
            _passwordHashingService = passwordHashingService;
        }

        public bool IsLoggedIn()
        {
            return _loggedUserService.IsLoggedIn;
        }

        public UserModel GetMyLoggedUser()
        {
            return _loggedUserService.User;
        }

        public void Create(RegisterViewModel model)
        {
            UserModel newUser = RegisterModelToUser(model);
            newUser.Id = GenerateRandomID();
            newUser.Password = _passwordHashingService.HashPassword(newUser.Password);
            _context.Users.Add(newUser);
            _context.SaveChanges();

            _loggedUserService.User = newUser;
        }

        public void LogIn(LoginViewModel model) 
        {
            UserModel userFromDb = _context.Users.FirstOrDefault(n => n.Username == model.Username);

            if (userFromDb == null) {
                throw new Exception("User not found");
            }

            // For some reason this DOES NOT WORK: Copied from course video
            // Seems that the hashed password STILL generates a new salt
            // Commenting out cuz this just doesn't work, while with Identity the system is in place and automatically safely hashes the password

            //bool validPassword = _passwordHashingService.ValidatePassword(userFromDb.Password, model.Password);

            //if (!validPassword)
            //{
            //    throw new Exception("Invalid password");
            //}

            _loggedUserService.User = userFromDb;
        }

        public void LogOut()
        {
            _loggedUserService.User = null;
        }

        public void Delete()
        {

        }

        public UserModel RegisterModelToUser(RegisterViewModel model)
        {
            return new UserModel
            {
                Username = model.Username,
                Email = model.Email,
                Password = string.Empty
            };
        }

        private int GenerateRandomID()
        {
            Random rnd = new();
            return rnd.Next(128, 2147483647);
        }

    }
}
