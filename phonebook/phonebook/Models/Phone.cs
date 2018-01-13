namespace phonebook.Models
{
    public class Phone:BaseEntity
    {
        public string PhoneNumber { get; set; }

        public int ContactID { get; set; }

        public virtual Contact Contact { get; set; }
    }
}