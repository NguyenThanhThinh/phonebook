using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace phonebook.Controllers
{
    using phonebook.ViewModels.Contacts;
    using phonebook.Repositories;
    using phonebook.Services;
    using phonebook.Models;
    using System.Linq.Expressions;
    using System.Web.Routing;
    using PagedList;
    using phonebook.ViewModels;
    using phonebook.Filters;

    [Authenticate]
    public class ContactController : Controller
    {
        private readonly ContactRepository contactRepo;
        public ContactController()
        {
            contactRepo = new ContactRepository();
        }
        public ActionResult Index()
        {
            ContactListViewModel model = new ContactListViewModel();

            TryUpdateModel(model);


            User user = AuthenticationService.LoggedUser;

            Expression<Func<Contact, bool>> filter = null;

            if (!string.IsNullOrEmpty(model.SearchString)) // With Searching
            {
                string[] searchArray = model.SearchString.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                filter = c => (c.UserID == user.Id) && (searchArray.Any(word => c.FirstName.Contains(word))
                                                     || searchArray.Any(word => c.LastName.Contains(word))
                                                     || searchArray.Any(word => c.Email.Contains(word)));
            }

            else // Without Searching
            {
                filter = c => c.UserID == user.Id;
            }

            model.Entities = contactRepo.GetAll(filter);

            // Sorting

            model.RouteDictionary = new RouteValueDictionary
            {
                { "SearchString", model.SearchString }
            };

            if (model.SortOrder == null)
            {
                model.SortOrder = ContactSorting.FirstNameAsc;
            }

            switch (model.SortOrder)
            {
                case ContactSorting.FirstNameAsc:
                default:
                    model.Entities = model.Entities.OrderBy(c => c.FirstName).ToList();
                    break;
                case ContactSorting.FirstNameDesc:
                    model.Entities = model.Entities.OrderByDescending(c => c.FirstName).ToList();
                    break;
                case ContactSorting.LastNameAsc:
                    model.Entities = model.Entities.OrderBy(c => c.LastName).ToList();
                    break;
                case ContactSorting.LastNameDesc:
                    model.Entities = model.Entities.OrderByDescending(c => c.LastName).ToList();
                    break;
                case ContactSorting.EmailAsc:
                    model.Entities = model.Entities.OrderBy(c => c.Email).ToList();
                    break;
                case ContactSorting.EmailDesc:
                    model.Entities = model.Entities.OrderByDescending(c => c.Email).ToList();
                    break;
            }

            // Paging

            int pageSize = 2;
            int pageNumber = (model.Page ?? 1);

            model.PagedContacts = new PagedList<Contact>(model.Entities, pageNumber, pageSize);

            return View(model);
        }

        public ActionResult Details(int? id)
        {
            if (id != null)
            {
                ContactRepository contactRepo = new ContactRepository();

                Contact contact = contactRepo.GetById(id.Value);

                if (contact != null && contact.UserID == AuthenticationService.LoggedUser.Id)
                {
                    ContactCreateEditViewModel model = new ContactCreateEditViewModel()
                    {
                        Id = contact.Id,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Email = contact.Email
                    };

                    GroupRepository groupRepo = new GroupRepository();

                    List<Group> contactGroups = contactRepo.GetAll().Where(c => c.Id == id).First().Groups;

                    if (contactGroups.Count > 0)
                    {
                        List<CheckBoxListItem> checkBoxListItems = new List<CheckBoxListItem>();

                        foreach (Group group in contactGroups)
                        {
                            checkBoxListItems.Add(new CheckBoxListItem()
                            {
                                Text = group.Name
                            });
                        }

                        model.Groups = checkBoxListItems;
                    }

                    return View(model);
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult CreateEdit(int? id)
        {
            if (id == null) // Create
            {
                ContactCreateEditViewModel model = new ContactCreateEditViewModel()
                {
                    UserID = AuthenticationService.LoggedUser.Id
                };

                GroupRepository groupRepo = new GroupRepository();

                List<Group> allGroups = groupRepo.GetAll(g => g.UserID == AuthenticationService.LoggedUser.Id);

                List<CheckBoxListItem> checkBoxListItems = new List<CheckBoxListItem>();

                foreach (Group group in allGroups)
                {
                    checkBoxListItems.Add(new CheckBoxListItem()
                    {
                        Id = group.Id,
                        Text = group.Name,
                        IsChecked = false
                    });
                }

                model.Groups = checkBoxListItems;

                return View(model);
            }

            if (id > 0) // Edit
            {
                ContactRepository contactRepo = new ContactRepository();

                Contact contact = contactRepo.GetById(id.Value);

                if (contact != null && contact.UserID == AuthenticationService.LoggedUser.Id)
                {
                    ContactCreateEditViewModel model = new ContactCreateEditViewModel()
                    {
                        Id = contact.Id,
                        UserID = contact.UserID,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Email = contact.Email
                    };

                    GroupRepository groupRepo = new GroupRepository();

                    List<Group> allGroups = groupRepo.GetAll(g => g.UserID == AuthenticationService.LoggedUser.Id);

                    //List<Group> contactGroups = groupRepo.GetAll(g => g.Contacts.Contains(contact));
                    List<Group> contactGroups = contactRepo.GetAll().Where(c => c.Id == id).First().Groups;

                    List<CheckBoxListItem> checkBoxListItems = new List<CheckBoxListItem>();

                    foreach (Group group in allGroups)
                    {
                        checkBoxListItems.Add(new CheckBoxListItem()
                        {
                            Id = group.Id,
                            Text = group.Name,
                           
                            IsChecked = contactGroups.Where(g => g.Id == group.Id).Any()
                        });
                    }

                    model.Groups = checkBoxListItems;

                    return View(model);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEdit(ContactCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Contact contact;

            ContactRepository contactRepo = new ContactRepository();

            if (model.Id > 0) // Edit
            {
                contact = contactRepo.GetById(model.Id);

                if (contact == null || contact.UserID != model.UserID)
                {
                    return HttpNotFound();
                }
            }

            else // Create
            {
                contact = new Contact()
                {
                    UserID = model.UserID,
                    Groups = new List<Group>()
                };
            }

            if (contact.UserID == AuthenticationService.LoggedUser.Id)
            {
                if (model.GroupIds != null)
                {
                    GroupRepository groupRepo = new GroupRepository();
                    groupRepo.Context = contactRepo.Context;
                    groupRepo.DbSet = groupRepo.Context.Set<Group>();

                    if (contact.Groups != null)
                    {
                        contact.Groups.Clear();
                    }

                    foreach (int groupId in model.GroupIds)
                    {
                        Group group = groupRepo.GetById(groupId);
                        contact.Groups.Add(group);
                    }
                }

                else
                {
                    contact.Groups.Clear();
                }

                contact.FirstName = model.FirstName;
                contact.LastName = model.LastName;
                contact.Email = model.Email;

                contactRepo.Save(contact);

                return RedirectToAction("Index");
            }

            return HttpNotFound();
        }

        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                ContactRepository contactRepo = new ContactRepository();

                Contact contact = contactRepo.GetById(id.Value);

                if (contact != null && contact.UserID == AuthenticationService.LoggedUser.Id)
                {
                    ContactCreateEditViewModel model = new ContactCreateEditViewModel()
                    {
                        Id = contact.Id,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Email = contact.Email
                    };

                    GroupRepository groupRepo = new GroupRepository();

                    List<Group> contactGroups = contactRepo.GetAll().Where(c => c.Id == id).First().Groups;

                    if (contactGroups.Count > 0)
                    {
                        List<CheckBoxListItem> checkBoxListItems = new List<CheckBoxListItem>();

                        foreach (Group group in contactGroups)
                        {
                            checkBoxListItems.Add(new CheckBoxListItem()
                            {
                                Text = group.Name
                            });
                        }

                        model.Groups = checkBoxListItems;
                    }

                    return View(model);
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
                ContactRepository contactRepo = new ContactRepository();

                Contact contact = contactRepo.GetById(id.Value);

                if (contact != null && contact.UserID == AuthenticationService.LoggedUser.Id)
                {
                    contact.Groups.Clear(); //one row took us like an hour to find!!! 
                    contactRepo.Delete(contact);

                    return RedirectToAction("Index");
                }
            }

            return HttpNotFound();
        }
    }
}