using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phonebook.ViewModels
{
    public class CheckBoxListItem:BaseViewModel
    {
        public string Text { get; set; }

        public bool IsChecked { get; set; }
    }
}