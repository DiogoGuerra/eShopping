namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePROMOtable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "Promo_ID", "dbo.Promotions");
            DropIndex("dbo.Products", new[] { "Promo_ID" });
            DropColumn("dbo.Products", "Promo_ID");
            DropForeignKey("dbo.Promotions", "ID", "dbo.Products");
            DropIndex("dbo.Promotions", new[] { "ID" });
            DropPrimaryKey("dbo.Promotions");
            DropTable("dbo.Promotions");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Promotions", "ProdutoID", c => c.Int(nullable: false));
            DropPrimaryKey("dbo.Promotions");
            AlterColumn("dbo.Promotions", "ID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Promotions", "ID");
            CreateIndex("dbo.Promotions", "ID");
            AddForeignKey("dbo.Promotions", "ID", "dbo.Products", "ProductID");
        }
    }
}
