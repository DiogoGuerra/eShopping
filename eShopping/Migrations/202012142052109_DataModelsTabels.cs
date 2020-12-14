namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataModelsTabels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nome_Categoria = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nome_Produto = c.String(nullable: false, maxLength: 50),
                        Stock = c.Int(nullable: false),
                        ID_Empresa = c.String(nullable: false),
                        EstaNoCatalogo = c.Boolean(nullable: false),
                        CategoriaID = c.Int(nullable: false),
                        Promo_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.CategoriaID, cascadeDelete: true)
                .ForeignKey("dbo.Promotions", t => t.Promo_ID)
                .Index(t => t.CategoriaID)
                .Index(t => t.Promo_ID);
            
            CreateTable(
                "dbo.Purchases",
                c => new
                    {
                        ID_Venda = c.Int(nullable: false, identity: true),
                        Preco_Total = c.Double(nullable: false),
                        ID_Cliente = c.String(nullable: false),
                        Data_Venda = c.DateTime(nullable: false),
                        EstaValidado = c.Boolean(nullable: false),
                        EstaEntregue = c.Boolean(nullable: false),
                        ID_Entrega = c.Int(nullable: false),
                        Entrega_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID_Venda)
                .ForeignKey("dbo.Deliveries", t => t.Entrega_ID)
                .Index(t => t.Entrega_ID);
            
            CreateTable(
                "dbo.Deliveries",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Tipo = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Promotions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TaxaPromocao = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PurchaseProducts",
                c => new
                    {
                        Purchase_ID_Venda = c.Int(nullable: false),
                        Products_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Purchase_ID_Venda, t.Products_ID })
                .ForeignKey("dbo.Purchases", t => t.Purchase_ID_Venda, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Products_ID, cascadeDelete: true)
                .Index(t => t.Purchase_ID_Venda)
                .Index(t => t.Products_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Promo_ID", "dbo.Promotions");
            DropForeignKey("dbo.PurchaseProducts", "Products_ID", "dbo.Products");
            DropForeignKey("dbo.PurchaseProducts", "Purchase_ID_Venda", "dbo.Purchases");
            DropForeignKey("dbo.Purchases", "Entrega_ID", "dbo.Deliveries");
            DropForeignKey("dbo.Products", "CategoriaID", "dbo.Categories");
            DropIndex("dbo.PurchaseProducts", new[] { "Products_ID" });
            DropIndex("dbo.PurchaseProducts", new[] { "Purchase_ID_Venda" });
            DropIndex("dbo.Purchases", new[] { "Entrega_ID" });
            DropIndex("dbo.Products", new[] { "Promo_ID" });
            DropIndex("dbo.Products", new[] { "CategoriaID" });
            DropTable("dbo.PurchaseProducts");
            DropTable("dbo.Promotions");
            DropTable("dbo.Deliveries");
            DropTable("dbo.Purchases");
            DropTable("dbo.Products");
            DropTable("dbo.Categories");
        }
    }
}
