namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataAnnotationCatedoriaIDRemoved : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "CategoriaID", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "CategoriaID" });
            AlterColumn("dbo.Products", "CategoriaID", c => c.Int());
            CreateIndex("dbo.Products", "CategoriaID");
            AddForeignKey("dbo.Products", "CategoriaID", "dbo.Categories", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "CategoriaID", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "CategoriaID" });
            AlterColumn("dbo.Products", "CategoriaID", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "CategoriaID");
            AddForeignKey("dbo.Products", "CategoriaID", "dbo.Categories", "ID", cascadeDelete: true);
        }
    }
}
