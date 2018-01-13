using System.Web.Mvc;

namespace phonebook.Controllers
{
    using phonebook.Models;
    using phonebook.Repositories;
    using phonebook.Services;
    using phonebook.ViewModels.Groups;
    public class GroupController : Controller
    {
        private readonly GroupRepository groupRepo;
        public GroupController()
        {
            groupRepo = new GroupRepository();
        }
        public ActionResult Index()
        {
            GroupListViewModel model = new GroupListViewModel();

            TryUpdateModel(model);

            model.Entities = groupRepo.GetAll(g => g.UserID == AuthenticationService.LoggedUser.Id);

            return View(model);
        }

        public ActionResult Details(int? id)
        {
            if (id != null)
            {
              
                Group group = groupRepo.GetById(id.Value);

                if (group != null && group.UserID == AuthenticationService.LoggedUser.Id)
                {
                    GroupsCreateEditViewModel g = new GroupsCreateEditViewModel()
                    {
                        Id = group.Id,
                        Name = group.Name,
                        UserID = group.UserID
                    };

                    return View(g);
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult CreateEdit(int? id)
        {
            if (id == null) // Create
            {
                return View(new GroupsCreateEditViewModel()
                {
                    UserID = AuthenticationService.LoggedUser.Id
                });
            }

            else if (id > 0) // Edit
            {

                Group group = groupRepo.GetById(id.Value);

                if (group != null && group.UserID == AuthenticationService.LoggedUser.Id)
                {
                    GroupsCreateEditViewModel g = new GroupsCreateEditViewModel()
                    {
                        Id = group.Id,
                        Name = group.Name,
                        UserID = group.UserID
                    };

                    return View(g);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEdit(GroupsCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Group group;

          
            if (model.Id > 0) // Edit
            {
                group = groupRepo.GetById(model.Id);

                if (group == null || group.UserID != model.UserID)
                {
                    return HttpNotFound();
                }
            }

            else // Create
            {
                group = new Group()
                {
                    UserID = AuthenticationService.LoggedUser.Id
                };
            }

            if (group.UserID == AuthenticationService.LoggedUser.Id)
            {
                group.Name = model.Name;

                groupRepo.Save(group);

                return RedirectToAction("Index");
            }

            return HttpNotFound();
        }

        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
              

                Group group = groupRepo.GetById(id.Value);

                if (group != null && group.UserID == AuthenticationService.LoggedUser.Id)
                {
                    GroupsCreateEditViewModel g = new GroupsCreateEditViewModel()
                    {
                        Id = group.Id,
                        Name = group.Name
                    };

                    return View(g);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id != null)
            {

                Group group = groupRepo.GetById(id.Value);

                if (group != null && group.UserID == AuthenticationService.LoggedUser.Id)
                {
                    group.Contacts.Clear();
                    groupRepo.Delete(group);

                    return RedirectToAction("Index");
                }
            }

            return HttpNotFound();
        }
    }
}