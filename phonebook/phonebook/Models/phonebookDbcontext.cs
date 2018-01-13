using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace phonebook.Models
{
    public class PhonebookDbcontext:DbContext
    {
        public PhonebookDbcontext():base("phonebook")
        {

        }
        public DbSet<User> Users { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Phone> Phones { get; set; }

        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }
    }
}