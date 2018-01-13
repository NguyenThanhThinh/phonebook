using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace phonebook.Controllers
{
    using phonebook.Filters;
    using phonebook.Models;
    using phonebook.Repositories;
    using phonebook.Services;
    using phonebook.ViewModels.Phones;

    [Authenticate]
    public class PhoneController : Controller
    {
        // Get contact by id if contact's user is logged in
        private Contact GetUserContact(int contactId)
        {
            ContactRepository contactRepo = new ContactRepository();

            Contact contact = contactRepo.GetById(contactId);

            return contact == null || contact.UserID != AuthenticationService.LoggedUser.Id ? null : contact;
        }

        // Check if contact's user is logged in
        private bool CheckIfContactsUserIsLogged(Phone phone)
        {
            Contact contact = GetUserContact(phone.ContactID);

            return contact != null;
        }

           

        public ActionResult Index(int? id) //contactID
        {
            if (id != null)
            {
                Contact contact = GetUserContact(id.Value);

                if (contact != null)
                {
                    PhoneListViewModel model = new PhoneListViewModel()
                    {
                        Contact = contact
                    };

                    model.Entities = model.Contact.Phones;

                    return View(model);
                }
            }

            return HttpNotFound();
        }

        public ActionResult Details(int? id) //phoneID
        {
            if (id != null)
            {
                PhoneRepository phoneRepo = new PhoneRepository();

                Phone phone = phoneRepo.GetById(id.Value);

                if (phone != null && CheckIfContactsUserIsLogged(phone))
                {
                    PhoneCreateEditViewModel p = new PhoneCreateEditViewModel()
                    {
                        Id = phone.Id,
                        Contact = phone.Contact,
                        PhoneNumber = phone.PhoneNumber
                    };

                    return View(p);
                }
            }

            return HttpNotFound();
        }

        public ActionResult CreateEdit(int? id, int? contactId)
        {
            if (id == null && contactId != null) // Create
            {
                Contact contact = GetUserContact(contactId.Value);

                if (contact != null)
                {
                    return View(new PhoneCreateEditViewModel()
                    {
                        Contact = contact
                    });
                }
            }

            if (id > 0) // Edit
            {
                PhoneRepository phoneRepo = new PhoneRepository();

                Phone phone = phoneRepo.GetById(id.Value);

                if (phone != null && CheckIfContactsUserIsLogged(phone))
                {
                    PhoneCreateEditViewModel p = new PhoneCreateEditViewModel()
                    {
                        Id = phone.Id,
                        Contact = phone.Contact,
                        ContactID = phone.ContactID,
                        PhoneNumber = phone.PhoneNumber
                    };

                    return View(p);
                }
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEdit(PhoneCreateEditViewModel model)
        {
            Contact contact = GetUserContact(model.ContactID);

            if (contact == null)
            {
                return HttpNotFound();
            }

            model.Contact = contact;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Phone phone;

            PhoneRepository phoneRepo = new PhoneRepository();

            if (model.Id > 0) // Edit
            {
                phone = phoneRepo.GetById(model.Id);

                if (phone == null || phone.ContactID != model.ContactID)
                {
                    return HttpNotFound();
                }
            }

            else // Create
            {
                phone = new Phone()
                {
                    ContactID = model.ContactID
                };
            }

            phone.PhoneNumber = model.PhoneNumber;

            phoneRepo.Save(phone);

            return RedirectToAction("Index", new { id = phone.ContactID });
        }

        public ActionResult Delete(int? id) // phoneID
        {
            if (id != null)
            {
                PhoneRepository phoneRepo = new PhoneRepository();

                Phone phone = phoneRepo.GetById(id.Value);

                if (phone != null && CheckIfContactsUserIsLogged(phone))
                {
                    PhoneCreateEditViewModel p = new PhoneCreateEditViewModel()
                    {
                        Id = phone.Id,
                        Contact = phone.Contact,
                        PhoneNumber = phone.PhoneNumber
                    };

                    return View(p);
                }
            }

            return HttpNotFound();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id) // phoneID
        {
            if (id != null)
            {
                PhoneRepository phoneRepo = new PhoneRepository();

                Phone phone = phoneRepo.GetById(id.Value);

                if (phone != null && CheckIfContactsUserIsLogged(phone))
                {
                    phoneRepo.Delete(phone);

                    return RedirectToAction("Index", new { id = phone.ContactID });
                }
            }

            return HttpNotFound();
        }
    }
}