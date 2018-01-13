using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phonebook.ViewModels.Users
{
    using PagedList;
    using phonebook.Models;
    public class UserListViewModel : BaseListViewModel<User>
    {
        public PagedList<User> PagedUsers { get; set; }

        public int? Page { get; set; }

        public string SearchString { get; set; }

        public UserSorting? SortOrder { get; set; }
    }
}