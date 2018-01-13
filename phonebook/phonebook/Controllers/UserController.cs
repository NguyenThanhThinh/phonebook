using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace phonebook.Controllers
{
    using phonebook.ViewModels.Users;
    using phonebook.Repositories;
    using phonebook.Models;
    using PagedList;
    using System.Web.Routing;

    public class UserController : Controller
    {

        public ActionResult Index()
        {
            UserListViewModel model = new UserListViewModel();

            UserRepository userRepo = new UserRepository();

            TryUpdateModel(model);

            model.Entities = userRepo.GetAll();

            if (!string.IsNullOrEmpty(model.SearchString)) // Search
            {
                model.Entities = userRepo.GetAll(u => u.FirstName.Contains(model.SearchString) ||
                                                      u.LastName.Contains(model.SearchString) ||
                                                      u.Username.Contains(model.SearchString) ||
                                                      u.Email.Contains(model.SearchString));
            }

            // Sorting

            model.RouteDictionary = new RouteValueDictionary
            {
                { "SearchString", model.SearchString }
            };

            if (model.SortOrder == null)
            {
                model.SortOrder = UserSorting.FirstNameAsc;
            }

            switch (model.SortOrder)
            {
                case UserSorting.FirstNameAsc:

                    model.Entities = model.Entities.OrderBy(u => u.FirstName).ToList();
                    break;

                case UserSorting.FirstNameDesc:
                    model.Entities = model.Entities.OrderByDescending(u => u.FirstName).ToList();
                    break;

                case UserSorting.LastNameAsc:
                    model.Entities = model.Entities.OrderBy(u => u.LastName).ToList();
                    break;

                case UserSorting.LastNameDesc:
                    model.Entities = model.Entities.OrderByDescending(u => u.LastName).ToList();
                    break;

                case UserSorting.UsernameAsc:
                    model.Entities = model.Entities.OrderBy(u => u.Username).ToList();
                    break;

                case UserSorting.UsernameDesc:
                    model.Entities = model.Entities.OrderByDescending(u => u.Username).ToList();
                    break;

                case UserSorting.EmailAsc:
                    model.Entities = model.Entities.OrderBy(u => u.Email).ToList();
                    break;

                case UserSorting.EmailDesc:
                    model.Entities = model.Entities.OrderByDescending(u => u.Email).ToList();
                    break;

                default:
                    model.Entities = model.Entities.OrderBy(u => u.FirstName).ToList();
                    break;
            }

            int pageSize = 3;

            int pageNumber = (model.Page ?? 1);

            model.PagedUsers = new PagedList<User>(model.Entities, pageNumber, pageSize);

            return View(model);
        }

        public ActionResult Details(int? id)
        {
            return View();
        }
    }
}