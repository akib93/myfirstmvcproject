namespace OnlineShopingManagment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dss : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Orders", "OrderDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "OrderDate", c => c.DateTime(nullable: false));
        }
    }
}
