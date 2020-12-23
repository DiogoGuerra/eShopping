namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCartItem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Items", "CarrinhoID", "dbo.Carts");
            DropForeignKey("dbo.Items", "Produto_ID", "dbo.Products");
            DropIndex("dbo.Items", new[] { "CarrinhoID" });
            DropIndex("dbo.Items", new[] { "Produto_ID" });
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
                        CarrinhoID = c.Int(nullable: false),
                        Produto_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ID_Cliente = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.Items", "Produto_ID");
            CreateIndex("dbo.Items", "CarrinhoID");
            AddForeignKey("dbo.Items", "Produto_ID", "dbo.Products", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Items", "CarrinhoID", "dbo.Carts", "ID", cascadeDelete: true);
        }
    }
}
