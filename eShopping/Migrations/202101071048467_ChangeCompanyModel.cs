namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeCompanyModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "Empresa_userID", "dbo.Companies");
            DropForeignKey("dbo.Orders", "Empresa_userID", "dbo.Companies");
            DropIndex("dbo.Products", new[] { "Empresa_userID" });
            DropIndex("dbo.Orders", new[] { "Empresa_userID" });
            RenameColumn(table: "dbo.Orders", name: "Empresa_userID", newName: "Empresa_CompanyId");
            RenameColumn(table: "dbo.Products", name: "Empresa_userID", newName: "CompanyId");
            DropPrimaryKey("dbo.Companies");
            AddColumn("dbo.Companies", "CompanyId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.AspNetUsers", "CompanyId", c => c.Int());
            AlterColumn("dbo.Products", "CompanyId", c => c.Int(nullable: false));
            AlterColumn("dbo.Orders", "Empresa_CompanyId", c => c.Int());
            AddPrimaryKey("dbo.Companies", "CompanyId");
            CreateIndex("dbo.Products", "CompanyId");
            CreateIndex("dbo.Orders", "Empresa_CompanyId");
            CreateIndex("dbo.AspNetUsers", "CompanyId");
            AddForeignKey("dbo.AspNetUsers", "CompanyId", "dbo.Companies", "CompanyId");
            AddForeignKey("dbo.Products", "CompanyId", "dbo.Companies", "CompanyId", cascadeDelete: true);
            AddForeignKey("dbo.Orders", "Empresa_CompanyId", "dbo.Companies", "CompanyId");
            DropColumn("dbo.Products", "ID_Empresa");
            DropColumn("dbo.Companies", "userID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "userID", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Products", "ID_Empresa", c => c.String(nullable: false));
            DropForeignKey("dbo.Orders", "Empresa_CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Products", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.AspNetUsers", "CompanyId", "dbo.Companies");
            DropIndex("dbo.AspNetUsers", new[] { "CompanyId" });
            DropIndex("dbo.Orders", new[] { "Empresa_CompanyId" });
            DropIndex("dbo.Products", new[] { "CompanyId" });
            DropPrimaryKey("dbo.Companies");
            AlterColumn("dbo.Orders", "Empresa_CompanyId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Products", "CompanyId", c => c.String(maxLength: 128));
            DropColumn("dbo.AspNetUsers", "CompanyId");
            DropColumn("dbo.Companies", "CompanyId");
            AddPrimaryKey("dbo.Companies", "userID");
            RenameColumn(table: "dbo.Products", name: "CompanyId", newName: "Empresa_userID");
            RenameColumn(table: "dbo.Orders", name: "Empresa_CompanyId", newName: "Empresa_userID");
            CreateIndex("dbo.Orders", "Empresa_userID");
            CreateIndex("dbo.Products", "Empresa_userID");
            AddForeignKey("dbo.Orders", "Empresa_userID", "dbo.Companies", "userID");
            AddForeignKey("dbo.Products", "Empresa_userID", "dbo.Companies", "userID");
        }
    }
}
