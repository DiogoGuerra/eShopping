namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCartAndItemModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ID_Cliente = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Quantidade = c.Int(nullable: false),
                        CarrinhoID = c.Int(nullable: false),
                        Produto_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Carts", t => t.CarrinhoID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Produto_ID, cascadeDelete: true)
                .Index(t => t.CarrinhoID)
                .Index(t => t.Produto_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "Produto_ID", "dbo.Products");
            DropForeignKey("dbo.Items", "CarrinhoID", "dbo.Carts");
            DropIndex("dbo.Items", new[] { "Produto_ID" });
            DropIndex("dbo.Items", new[] { "CarrinhoID" });
            DropTable("dbo.Items");
            DropTable("dbo.Carts");
        }
    }
}
