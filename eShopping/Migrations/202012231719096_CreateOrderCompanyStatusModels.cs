namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateOrderCompanyStatusModels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Purchases", "Entrega_ID", "dbo.Deliveries");
            RenameTable(name: "dbo.Purchases", newName: "Orders");
            DropForeignKey("dbo.PurchaseProducts", "Purchase_ID_Venda", "dbo.Purchases");
            DropForeignKey("dbo.PurchaseProducts", "Products_ID", "dbo.Products");
            DropIndex("dbo.Orders", new[] { "Entrega_ID" });
            DropIndex("dbo.PurchaseProducts", new[] { "Purchase_ID_Venda" });
            DropIndex("dbo.PurchaseProducts", new[] { "Products_ID" });
            RenameColumn(table: "dbo.Orders", name: "Entrega_ID", newName: "EntregaID");
            DropPrimaryKey("dbo.Products");
            DropPrimaryKey("dbo.Orders");
            DropColumn("dbo.Products", "ID");
            DropColumn("dbo.Orders", "ID_Venda");
            DropColumn("dbo.Orders", "Preco_Total");
            DropColumn("dbo.Orders", "EstaValidado");
            DropColumn("dbo.Orders", "EstaEntregue");
            DropColumn("dbo.Orders", "ID_Entrega");
            DropTable("dbo.PurchaseProducts");


            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProductsOrders",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        OrderID = c.Int(nullable: false),
                        Quantidade = c.Int(nullable: false),
                        Preco_Produto = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductID, t.OrderID })
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.OrderID);
            
            AddColumn("dbo.Products", "ProductID", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Orders", "OrderID", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Orders", "EstaFinalizado", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "Empresa_ID", c => c.Int());
            AddColumn("dbo.Orders", "Estado_ID", c => c.Int());
            AddColumn("dbo.Orders", "Products_ProductID", c => c.Int());
            AlterColumn("dbo.Orders", "EntregaID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Products", "ProductID");
            AddPrimaryKey("dbo.Orders", "OrderID");
            CreateIndex("dbo.Orders", "EntregaID");
            CreateIndex("dbo.Orders", "Empresa_ID");
            CreateIndex("dbo.Orders", "Estado_ID");
            CreateIndex("dbo.Orders", "Products_ProductID");
            AddForeignKey("dbo.Orders", "Empresa_ID", "dbo.Companies", "ID");
            AddForeignKey("dbo.Orders", "Estado_ID", "dbo.Status", "ID");
            AddForeignKey("dbo.Orders", "Products_ProductID", "dbo.Products", "ProductID");
            AddForeignKey("dbo.Orders", "EntregaID", "dbo.Deliveries", "ID", cascadeDelete: true);
           ;
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PurchaseProducts",
                c => new
                    {
                        Purchase_ID_Venda = c.Int(nullable: false),
                        Products_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Purchase_ID_Venda, t.Products_ID });
            
            AddColumn("dbo.Orders", "ID_Entrega", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "EstaEntregue", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "EstaValidado", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "Preco_Total", c => c.Double(nullable: false));
            AddColumn("dbo.Orders", "ID_Venda", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Products", "ID", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Orders", "EntregaID", "dbo.Deliveries");
            DropForeignKey("dbo.ProductsOrders", "ProductID", "dbo.Products");
            DropForeignKey("dbo.ProductsOrders", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.Orders", "Products_ProductID", "dbo.Products");
            DropForeignKey("dbo.Orders", "Estado_ID", "dbo.Status");
            DropForeignKey("dbo.Orders", "Empresa_ID", "dbo.Companies");
            DropIndex("dbo.ProductsOrders", new[] { "OrderID" });
            DropIndex("dbo.ProductsOrders", new[] { "ProductID" });
            DropIndex("dbo.Orders", new[] { "Products_ProductID" });
            DropIndex("dbo.Orders", new[] { "Estado_ID" });
            DropIndex("dbo.Orders", new[] { "Empresa_ID" });
            DropIndex("dbo.Orders", new[] { "EntregaID" });
            DropPrimaryKey("dbo.Orders");
            DropPrimaryKey("dbo.Products");
            AlterColumn("dbo.Orders", "EntregaID", c => c.Int());
            DropColumn("dbo.Orders", "Products_ProductID");
            DropColumn("dbo.Orders", "Estado_ID");
            DropColumn("dbo.Orders", "Empresa_ID");
            DropColumn("dbo.Orders", "EstaFinalizado");
            DropColumn("dbo.Orders", "OrderID");
            DropColumn("dbo.Products", "ProductID");
            DropTable("dbo.ProductsOrders");
            DropTable("dbo.Status");
            DropTable("dbo.Companies");
            AddPrimaryKey("dbo.Orders", "ID_Venda");
            AddPrimaryKey("dbo.Products", "ID");
            RenameColumn(table: "dbo.Orders", name: "EntregaID", newName: "Entrega_ID");
            CreateIndex("dbo.PurchaseProducts", "Products_ID");
            CreateIndex("dbo.PurchaseProducts", "Purchase_ID_Venda");
            CreateIndex("dbo.Orders", "Entrega_ID");
            AddForeignKey("dbo.Purchases", "Entrega_ID", "dbo.Deliveries", "ID");
            AddForeignKey("dbo.PurchaseProducts", "Products_ID", "dbo.Products", "ID", cascadeDelete: true);
            AddForeignKey("dbo.PurchaseProducts", "Purchase_ID_Venda", "dbo.Purchases", "ID_Venda", cascadeDelete: true);
            RenameTable(name: "dbo.Orders", newName: "Purchases");
        }
    }
}
