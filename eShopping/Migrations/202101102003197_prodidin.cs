namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class prodidin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promotions", "ProdutoID", c => c.Int(nullable: false));
            CreateIndex("dbo.Promotions", "ProdutoID");
            AddForeignKey("dbo.Promotions", "ProdutoID", "dbo.Products", "ProductID", cascadeDelete: true);
            

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Promotions", "ProdutoID", "dbo.Products");
            DropIndex("dbo.Promotions", new[] { "ProdutoID" });
            DropColumn("dbo.Promotions", "ProdutoID");
        }
    }
}
