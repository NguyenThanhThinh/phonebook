namespace phonebook.ViewModels.Phones
{
    using phonebook.Models;
    public class PhoneListViewModel:BaseListViewModel<Phone>
    {
        public Contact Contact { get; set; }
    }
}