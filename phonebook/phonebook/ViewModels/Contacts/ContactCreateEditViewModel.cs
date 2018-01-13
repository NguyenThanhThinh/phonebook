using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace phonebook.ViewModels.Contacts
{
    using phonebook.Models;
    public class ContactCreateEditViewModel:BaseViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public int UserID { get; set; }

        public User User { get; set; }

        public List<CheckBoxListItem> Groups { get; set; }

        public int[] GroupIds { get; set; }

        public ContactCreateEditViewModel()
        {
            Groups = new List<CheckBoxListItem>();
        }
    }
}