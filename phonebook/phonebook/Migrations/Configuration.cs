namespace phonebook.Migrations
{
    using phonebook.Models;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<phonebook.Models.PhonebookDbcontext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(phonebook.Models.PhonebookDbcontext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Users.AddOrUpdate(n => n.Username, new User
            {
                Username="admin",
                Password="123456",
                Email="thanhthinhcntt@gmail.com",
                IsAdmin=true
            });
        }
    }
}
