﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PerfectedCheck.Models;
using PerfectedCheck.Services;

namespace PerfectedCheck.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userManager;

        public AccountController(UserService userManager)
        {
            _userManager = userManager;
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
        public IActionResult Register(RegisterViewModel model)
        {
            _userManager.Create(model);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                _userManager.LogIn(model);
                return RedirectToAction("Index", "Home");

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            _userManager.LogOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
