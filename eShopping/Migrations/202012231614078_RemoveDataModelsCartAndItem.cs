namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDataModelsCartAndItem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Items", "Produto_ID", "dbo.Products");
            DropForeignKey("dbo.Items", "Cart_ID", "dbo.Carts");
            DropIndex("dbo.Items", new[] { "Produto_ID" });
            DropIndex("dbo.Items", new[] { "Cart_ID" });
            DropTable("dbo.Carts");
            DropTable("dbo.Items");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Quantidade = c.Int(nullable: false),
                        UniPreco = c.Double(nullable: false),
                        TotalPrice = c.Double(nullable: false),
                        Carrinho_ID = c.Int(nullable: false),
                        Produto_ID = c.Int(nullable: false),
                        Cart_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.Items", "Cart_ID");
            CreateIndex("dbo.Items", "Produto_ID");
            AddForeignKey("dbo.Items", "Cart_ID", "dbo.Carts", "ID");
            AddForeignKey("dbo.Items", "Produto_ID", "dbo.Products", "ID", cascadeDelete: true);
        }
    }
}
