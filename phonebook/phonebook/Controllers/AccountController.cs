using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace phonebook.Controllers
{
    using phonebook.ViewModels.Account;
    using phonebook.Services;
    using phonebook.Repositories;
    using phonebook.Models;

    public class AccountController : Controller
    {
        public ActionResult Login(string RedirectUrl)
        {
            return View(new LoginViewModel
            {
                RedirectUrl=RedirectUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            AuthenticationService.AuthenticateUser(model.Username, model.Password);

            if (AuthenticationService.IsLogged)
            {
                if (string.IsNullOrWhiteSpace(model.RedirectUrl))
                {
                    return RedirectToAction("Index", "Home");
                }

                return Redirect(model.RedirectUrl);
            }

            return View();

        }

        public ActionResult Logout()
        {
            AuthenticationService.LogOut();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(AccountRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserRepository userRepo = new UserRepository();

                User user = new User()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Password = model.Password
                };

                bool userDoesntExist = true;

                User userUsername = userRepo.GetAll(us => us.Username == model.Username).FirstOrDefault();

                if (userUsername != null)
                {
                    ModelState.AddModelError("Username", "Username already exists");

                    userDoesntExist = false;
                }

                user.Username = model.Username;

                User userEmail = userRepo.GetAll(us => us.Email == model.Email).FirstOrDefault();

                if (userEmail != null)
                {
                    ModelState.AddModelError("Email", "Email already exists");

                    userDoesntExist = false;
                }

                user.Email = model.Email;

                if (userDoesntExist)
                {
                    userRepo.Save(user);

                    return RedirectToAction("Login");
                }
            }

            return View(model);
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}