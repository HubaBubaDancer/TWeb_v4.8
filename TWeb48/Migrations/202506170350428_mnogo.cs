namespace TWeb48.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mnogo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Coupons",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(),
                        Description = c.String(),
                        DiscountInPercents = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValidUntil = c.DateTime(nullable: false),
                        ValidFrom = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Coupons");
        }
    }
}
