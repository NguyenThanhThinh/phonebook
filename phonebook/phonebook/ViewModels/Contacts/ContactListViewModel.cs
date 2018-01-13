using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phonebook.ViewModels.Contacts
{
    using PagedList;
    using phonebook.Models;
    public class ContactListViewModel:BaseListViewModel<Contact>
    {
        public PagedList<Contact> PagedContacts { get; set; }

        public int? Page { get; set; }

        public string SearchString { get; set; }

        public ContactSorting? SortOrder { get; set; }
    }
}