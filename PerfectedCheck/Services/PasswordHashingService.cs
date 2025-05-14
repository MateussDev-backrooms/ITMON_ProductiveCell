using Microsoft.AspNetCore.Identity;

namespace PerfectedCheck.Services
{
    public class PasswordHashingService
    {
        private PasswordHasher<string> hasher = new PasswordHasher<string>();

        public string HashPassword(string password)
        {
            return hasher.HashPassword(null, password);
        }

        public bool ValidatePassword(string hashedPassword, string input)
        {
            var result = hasher.VerifyHashedPassword(null, hashedPassword, input);
            return result == PasswordVerificationResult.Success;
        }
    }
}
