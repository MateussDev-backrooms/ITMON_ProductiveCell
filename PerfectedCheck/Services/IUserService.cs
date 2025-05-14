using PerfectedCheck.Models;

namespace PerfectedCheck.Services
{
    public interface IUserService
    {
        public void Create(RegisterViewModel model);
        public void LogIn(LoginViewModel model);
        public void Delete();
    }
}
