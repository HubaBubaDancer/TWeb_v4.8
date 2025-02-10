namespace TWeb48.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class redoCar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "PhotoPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cars", "PhotoPath");
        }
    }
}
