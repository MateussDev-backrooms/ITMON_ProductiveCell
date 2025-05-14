using PerfectedCheck.Models;

namespace PerfectedCheck.Services
{
    public class LoggedUserService
    {
        private UserModel user;
        public UserModel User
        {
            get => this.user;
            set
            {
                user = value;
                IsLoggedIn = user != null;
            }
        }

        public bool IsLoggedIn;
    }
}
