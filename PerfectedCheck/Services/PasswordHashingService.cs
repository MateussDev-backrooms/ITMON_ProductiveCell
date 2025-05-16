using Microsoft.AspNetCore.Identity;

namespace PerfectedCheck.Services
{
    public class PasswordHashingService
    {
        private readonly PasswordHasher<string> hasher = new PasswordHasher<string>();

        public string HashPassword(string password)
        {
            return hasher.HashPassword("the_user", password);
        }

        public bool ValidatePassword(string hashedPassword, string input)
        {
            var result = hasher.VerifyHashedPassword("the_user", hashedPassword, input);
            return result == PasswordVerificationResult.Success;
        }
    }
}
