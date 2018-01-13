using System.Linq;
using System.Web.Mvc;

namespace phonebook.Controllers
{
    using phonebook.ViewModels.Account;
    using phonebook.Services;
    using phonebook.Repositories;
    using phonebook.Models;
    using System.IO;

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
        [Route("dang-xuat.html")]
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

        public ActionResult MyProfile()
        {
            UserRepository userRepo = new UserRepository();

            User user = userRepo.GetById(AuthenticationService.LoggedUser.Id);

            AccountProfileViewModel p = new AccountProfileViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Password = user.Password,
                Email = user.Email
            };

            if (user.ImageName != null)
            {
                p.ImageName = user.ImageName;
            }

            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyProfile(AccountProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserRepository userRepo = new UserRepository();

                User user = userRepo.GetById(model.Id);

                user.FirstName = model.FirstName;

                user.LastName = model.LastName;

                user.Password = model.Password;

                if (model.Image != null && model.Image.ContentLength > 0)
                {
                    if (model.Image.ContentType.Contains("image"))
                    {
                        model.ImageName = Path.GetFileName(model.Image.FileName);

                        string imagePath = Server.MapPath("~/Images/" + model.ImageName);

                        model.Image.SaveAs(imagePath);

                        user.ImageName = model.ImageName;
                    }

                    else
                    {
                        ModelState.AddModelError(string.Empty, "Uploaded file isn't an image!");

                        return View(model);
                    }
                }

                bool userDoesntExist = true;

                if (model.Username != user.Username)
                {
                    User userUsername = userRepo.GetAll(us => us.Username == model.Username).FirstOrDefault();

                    if (userUsername != null)
                    {
                        ModelState.AddModelError("Username", "Username already exists");

                        userDoesntExist = false;
                    }

                    user.Username = model.Username;
                }

                if (model.Email != user.Email)
                {
                    User userEmail = userRepo.GetAll(us => us.Email == model.Email).FirstOrDefault();

                    if (userEmail != null)
                    {
                        ModelState.AddModelError("Email", "Email already exists");

                        userDoesntExist = false;
                    }

                    user.Email = model.Email;
                }

                if (userDoesntExist)
                {
                    userRepo.Save(user);

                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }

        public ActionResult DeletePicture(string imageName, int? id)
        {
            string imagePath = Request.MapPath("~/Images/" + imageName);

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);

                UserRepository userRepo = new UserRepository();

                User user = userRepo.GetById(id.Value);

                user.ImageName = null;

                userRepo.Save(user);

                return RedirectToAction("MyProfile");
            }

            return HttpNotFound();
        }
    }
}