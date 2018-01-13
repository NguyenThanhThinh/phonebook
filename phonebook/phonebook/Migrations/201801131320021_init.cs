namespace phonebook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Username = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                        IsAdmin = c.Boolean(nullable: false),
                        ImageName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Phones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PhoneNumber = c.String(),
                        ContactID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contacts", t => t.ContactID, cascadeDelete: true)
                .Index(t => t.ContactID);
            
            CreateTable(
                "dbo.GroupContacts",
                c => new
                    {
                        Group_Id = c.Int(nullable: false),
                        Contact_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Group_Id, t.Contact_Id })
                .ForeignKey("dbo.Groups", t => t.Group_Id)
                .ForeignKey("dbo.Contacts", t => t.Contact_Id)
                .Index(t => t.Group_Id)
                .Index(t => t.Contact_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contacts", "UserID", "dbo.Users");
            DropForeignKey("dbo.Phones", "ContactID", "dbo.Contacts");
            DropForeignKey("dbo.Groups", "UserID", "dbo.Users");
            DropForeignKey("dbo.GroupContacts", "Contact_Id", "dbo.Contacts");
            DropForeignKey("dbo.GroupContacts", "Group_Id", "dbo.Groups");
            DropIndex("dbo.GroupContacts", new[] { "Contact_Id" });
            DropIndex("dbo.GroupContacts", new[] { "Group_Id" });
            DropIndex("dbo.Phones", new[] { "ContactID" });
            DropIndex("dbo.Groups", new[] { "UserID" });
            DropIndex("dbo.Contacts", new[] { "UserID" });
            DropTable("dbo.GroupContacts");
            DropTable("dbo.Phones");
            DropTable("dbo.Users");
            DropTable("dbo.Groups");
            DropTable("dbo.Contacts");
        }
    }
}
