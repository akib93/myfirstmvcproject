namespace OnlineShopingManagment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class trrrq : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Invoices", "OrderID", "dbo.Orders");
            DropIndex("dbo.Invoices", new[] { "OrderID" });
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        Productname = c.String(),
                        price = c.Single(nullable: false),
                        qty = c.Int(nullable: false),
                        bill = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ProductId);
            
            AddColumn("dbo.Orders", "InvoiceID", c => c.Int(nullable: false));
            CreateIndex("dbo.Orders", "InvoiceID");
            AddForeignKey("dbo.Orders", "InvoiceID", "dbo.Invoices", "InvoiceID", cascadeDelete: true);
            DropColumn("dbo.Invoices", "OrderID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Invoices", "OrderID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Orders", "InvoiceID", "dbo.Invoices");
            DropIndex("dbo.Orders", new[] { "InvoiceID" });
            DropColumn("dbo.Orders", "InvoiceID");
            DropTable("dbo.Carts");
            CreateIndex("dbo.Invoices", "OrderID");
            AddForeignKey("dbo.Invoices", "OrderID", "dbo.Orders", "OrderID", cascadeDelete: true);
        }
    }
}
