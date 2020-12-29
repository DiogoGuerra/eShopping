namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCartModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductsOrders", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.ProductsOrders", "ProductID", "dbo.Products");
            DropIndex("dbo.ProductsOrders", new[] { "OrderID" });
            RenameColumn(table: "dbo.ProductsOrders", name: "OrderID", newName: "Order_OrderID");
            DropPrimaryKey("dbo.ProductsOrders");
            AlterColumn("dbo.ProductsOrders", "Order_OrderID", c => c.Int());
            AddPrimaryKey("dbo.ProductsOrders", "ProductID");
            CreateIndex("dbo.ProductsOrders", "Order_OrderID");
            AddForeignKey("dbo.ProductsOrders", "Order_OrderID", "dbo.Orders", "OrderID");
            AddForeignKey("dbo.ProductsOrders", "ProductID", "dbo.Products", "ProductID");
            DropColumn("dbo.Orders", "EstaFinalizado");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "EstaFinalizado", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.ProductsOrders", "ProductID", "dbo.Products");
            DropForeignKey("dbo.ProductsOrders", "Order_OrderID", "dbo.Orders");
            DropIndex("dbo.ProductsOrders", new[] { "Order_OrderID" });
            DropPrimaryKey("dbo.ProductsOrders");
            AlterColumn("dbo.ProductsOrders", "Order_OrderID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ProductsOrders", new[] { "ProductID", "OrderID" });
            RenameColumn(table: "dbo.ProductsOrders", name: "Order_OrderID", newName: "OrderID");
            CreateIndex("dbo.ProductsOrders", "OrderID");
            AddForeignKey("dbo.ProductsOrders", "ProductID", "dbo.Products", "ProductID", cascadeDelete: true);
            AddForeignKey("dbo.ProductsOrders", "OrderID", "dbo.Orders", "OrderID", cascadeDelete: true);
        }
    }
}
