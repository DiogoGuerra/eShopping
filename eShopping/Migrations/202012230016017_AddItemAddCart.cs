namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddItemAddCart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Quantidade = c.Int(nullable: false),
                        Carrinho_ID = c.Int(nullable: false),
                        Produto_ID = c.Int(nullable: false),
                        Cart_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.Produto_ID, cascadeDelete: true)
                .ForeignKey("dbo.Carts", t => t.Cart_ID)
                .Index(t => t.Produto_ID)
                .Index(t => t.Cart_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "Cart_ID", "dbo.Carts");
            DropForeignKey("dbo.Items", "Produto_ID", "dbo.Products");
            DropIndex("dbo.Items", new[] { "Cart_ID" });
            DropIndex("dbo.Items", new[] { "Produto_ID" });
            DropTable("dbo.Items");
            DropTable("dbo.Carts");
        }
    }
}
