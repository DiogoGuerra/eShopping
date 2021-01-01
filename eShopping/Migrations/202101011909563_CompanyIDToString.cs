namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyIDToString : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "Empresa_ID", "dbo.Companies");
            DropIndex("dbo.Orders", new[] { "Empresa_ID" });
            RenameColumn(table: "dbo.Orders", name: "Empresa_ID", newName: "Empresa_userID");
            DropPrimaryKey("dbo.Companies");
            AlterColumn("dbo.Companies", "userID", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Orders", "Empresa_userID", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.Companies", "userID");
            CreateIndex("dbo.Orders", "Empresa_userID");
            AddForeignKey("dbo.Orders", "Empresa_userID", "dbo.Companies", "userID");
            DropColumn("dbo.Companies", "ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "ID", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Orders", "Empresa_userID", "dbo.Companies");
            DropIndex("dbo.Orders", new[] { "Empresa_userID" });
            DropPrimaryKey("dbo.Companies");
            AlterColumn("dbo.Orders", "Empresa_userID", c => c.Int());
            AlterColumn("dbo.Companies", "userID", c => c.String());
            AddPrimaryKey("dbo.Companies", "ID");
            RenameColumn(table: "dbo.Orders", name: "Empresa_userID", newName: "Empresa_ID");
            CreateIndex("dbo.Orders", "Empresa_ID");
            AddForeignKey("dbo.Orders", "Empresa_ID", "dbo.Companies", "ID");
        }
    }
}
