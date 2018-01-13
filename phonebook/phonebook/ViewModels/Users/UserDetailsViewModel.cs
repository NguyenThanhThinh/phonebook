using System.ComponentModel.DataAnnotations;

namespace phonebook.ViewModels.Users
{
    public class UserDetailsViewModel:BaseViewModel
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Profile Picture")]
        public string ImageName { get; set; }
    }
}