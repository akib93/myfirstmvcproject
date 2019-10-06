namespace OnlineShopingManagment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class trrrql : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invoices", "TotalBill", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invoices", "TotalBill");
        }
    }
}
