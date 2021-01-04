namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmptyMigration : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Products", new[] { "Empresa_userID" });
            DropIndex("dbo.Products", new[] { "Empresa_ID" });
            DropForeignKey("dbo.Products", "Empresa_userID", "dbo.Companies");
            DropForeignKey("dbo.Products", "Empresa_ID", "dbo.Companies");
            DropColumn("dbo.Products", "Empresa_ID");
            AddColumn("dbo.Products", "ID_Empresa", c => c.String(nullable: false));
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Empresa_ID", c => c.String());
            DropColumn("dbo.Products", "ID_Empresa");
            CreateIndex("dbo.Products", "Empresa_userID");
            CreateIndex("dbo.Products", "Empresa_ID");
            AddForeignKey("dbo.Products", "Empresa_userID", "dbo.Companies", "userID");
            AddForeignKey("dbo.Products", "Empresa_ID", "dbo.Companies", "userID");
        }
    }
}
