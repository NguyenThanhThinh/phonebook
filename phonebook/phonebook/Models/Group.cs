using System.Collections.Generic;

namespace phonebook.Models
{
    public class Group : BaseEntity
    {
        public string Name { get; set; }

        public int UserID { get; set; }

        public virtual User User { get; set; }

        public virtual List<Contact> Contacts { get; set; }
    }
}