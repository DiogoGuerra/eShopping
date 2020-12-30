namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewModelsWithProductOrder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "Products_ProductID", "dbo.Products");
            DropForeignKey("dbo.ProductsOrders", "Cart_ID", "dbo.Carts");
            DropForeignKey("dbo.ProductsOrders", "ProductID", "dbo.Products");
            DropForeignKey("dbo.ProductsOrders", "Order_OrderID", "dbo.Orders");
            DropIndex("dbo.ProductsOrders", new[] { "Order_OrderID" });
            DropIndex("dbo.ProductsOrders", new[] { "Cart_ID" });
            DropIndex("dbo.Orders", new[] { "Products_ProductID" });
            RenameColumn(table: "dbo.ProductsOrders", name: "Order_OrderID", newName: "OrderID");
            DropPrimaryKey("dbo.ProductsOrders");
            AddColumn("dbo.Orders", "PedidoEmAberto", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ProductsOrders", "OrderID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ProductsOrders", new[] { "ProductID", "OrderID" });
            CreateIndex("dbo.ProductsOrders", "OrderID");
            AddForeignKey("dbo.ProductsOrders", "ProductID", "dbo.Products", "ProductID", cascadeDelete: true);
            AddForeignKey("dbo.ProductsOrders", "OrderID", "dbo.Orders", "OrderID", cascadeDelete: true);
            DropColumn("dbo.ProductsOrders", "Cart_ID");
            DropColumn("dbo.Orders", "Products_ProductID");
            DropTable("dbo.Carts");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Orders", "Products_ProductID", c => c.Int());
            AddColumn("dbo.ProductsOrders", "Cart_ID", c => c.Int());
            DropForeignKey("dbo.ProductsOrders", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.ProductsOrders", "ProductID", "dbo.Products");
            DropIndex("dbo.ProductsOrders", new[] { "OrderID" });
            DropPrimaryKey("dbo.ProductsOrders");
            AlterColumn("dbo.ProductsOrders", "OrderID", c => c.Int());
            DropColumn("dbo.Orders", "PedidoEmAberto");
            AddPrimaryKey("dbo.ProductsOrders", "ProductID");
            RenameColumn(table: "dbo.ProductsOrders", name: "OrderID", newName: "Order_OrderID");
            CreateIndex("dbo.Orders", "Products_ProductID");
            CreateIndex("dbo.ProductsOrders", "Cart_ID");
            CreateIndex("dbo.ProductsOrders", "Order_OrderID");
            AddForeignKey("dbo.ProductsOrders", "Order_OrderID", "dbo.Orders", "OrderID");
            AddForeignKey("dbo.ProductsOrders", "ProductID", "dbo.Products", "ProductID");
            AddForeignKey("dbo.ProductsOrders", "Cart_ID", "dbo.Carts", "ID");
            AddForeignKey("dbo.Orders", "Products_ProductID", "dbo.Products", "ProductID");
        }
    }
}
