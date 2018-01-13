using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace phonebook.ViewModels.Groups
{
    using phonebook.Models;
    public class GroupsCreateEditViewModel:BaseViewModel
    {
        [Required]
        public string Name { get; set; }

        public int UserID { get; set; }

        public User User { get; set; }

        public List<Group> Groups { get; set; }
    }
}