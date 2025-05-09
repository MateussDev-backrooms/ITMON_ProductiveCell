using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PerfectedCheck.Models;

namespace PerfectedCheck.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;

        public AccountController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new UserModel { UserName = model.Username, Email = model.Email };
                var register_result = await _userManager.CreateAsync(user, model.Password);

                if (register_result.Succeeded)
                {
                    // User successfully signed in
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    Console.WriteLine("Successfully signed in");
                    return RedirectToAction("Index", "Home");
                } else
                {
                    Console.WriteLine("Failed to register:");
                    foreach(var err in register_result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                        Console.WriteLine($"Error: {err.Code} - {err.Description}");
                    }
                }

            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var login_result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

                if (login_result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to log in");
                }

            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
