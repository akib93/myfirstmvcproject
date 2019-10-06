namespace OnlineShopingManagment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class looo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShoppingCartViewModels",
                c => new
                    {
                        ShoppingCartViewModelID = c.Int(nullable: false, identity: true),
                        CartTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ShoppingCartViewModelID);
            
            AddColumn("dbo.Carts", "Count", c => c.Int(nullable: false));
            AddColumn("dbo.Carts", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Carts", "RecordId", c => c.Int(nullable: false));
            AddColumn("dbo.Carts", "Quentity", c => c.Int(nullable: false));
            AddColumn("dbo.Carts", "ShoppingCartViewModel_ShoppingCartViewModelID", c => c.Int());
            AddColumn("dbo.Orders", "OrderDate", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Carts", "ShoppingCartViewModel_ShoppingCartViewModelID");
            AddForeignKey("dbo.Carts", "ShoppingCartViewModel_ShoppingCartViewModelID", "dbo.ShoppingCartViewModels", "ShoppingCartViewModelID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Carts", "ShoppingCartViewModel_ShoppingCartViewModelID", "dbo.ShoppingCartViewModels");
            DropIndex("dbo.Carts", new[] { "ShoppingCartViewModel_ShoppingCartViewModelID" });
            DropColumn("dbo.Orders", "OrderDate");
            DropColumn("dbo.Carts", "ShoppingCartViewModel_ShoppingCartViewModelID");
            DropColumn("dbo.Carts", "Quentity");
            DropColumn("dbo.Carts", "RecordId");
            DropColumn("dbo.Carts", "DateCreated");
            DropColumn("dbo.Carts", "Count");
            DropTable("dbo.ShoppingCartViewModels");
        }
    }
}
