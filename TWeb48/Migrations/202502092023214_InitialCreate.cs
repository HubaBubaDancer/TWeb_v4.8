namespace TWeb48.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Car", newName: "Cars");
            RenameTable(name: "dbo.Role", newName: "Roles");
            RenameTable(name: "dbo.UserRole", newName: "UserRoles");
            RenameTable(name: "dbo.User", newName: "Users");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Users", newName: "User");
            RenameTable(name: "dbo.UserRoles", newName: "UserRole");
            RenameTable(name: "dbo.Roles", newName: "Role");
            RenameTable(name: "dbo.Cars", newName: "Car");
        }
    }
}
