namespace OnlineShopingManagment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class opo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Carts", "Firstname", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Carts", "Firstname");
        }
    }
}
