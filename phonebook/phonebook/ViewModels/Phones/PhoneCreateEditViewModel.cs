using System.ComponentModel.DataAnnotations;

namespace phonebook.ViewModels.Phones
{
    using phonebook.Models;
    public class PhoneCreateEditViewModel:BaseViewModel
    {
        [Required]
        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage = "The phone number must contain only digits")]
        public string PhoneNumber { get; set; }

        public int ContactID { get; set; }

        public Contact Contact { get; set; }
    }
}