namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class temdedar : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "ID_Empresa", "dbo.Companies");
            DropIndex("dbo.Products", new[] { "ID_Empresa" });
            DropColumn("dbo.Products", "ID_Empresa");
            AddColumn("dbo.Products", "Empresa_userID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Products", "Empresa_userID");
            AddForeignKey("dbo.Products", "Empresa_userID", "dbo.Companies", "userID");
           

        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "ID_Empresa");
            DropForeignKey("dbo.Orders", "Empresa_userID", "dbo.Companies");
            DropIndex("dbo.Orders", new[] { "Empresa_userID" });
            DropColumn("dbo.Products", "Empresa_userID");
            AddColumn("dbo.Products", "ID_Empresa", c => c.String(nullable: false));
            CreateIndex("dbo.Products", "ID_Empresa");
            AddForeignKey("dbo.Products", "ID_Empresa", "dbo.Companies", "userID");
        }
    }
}
