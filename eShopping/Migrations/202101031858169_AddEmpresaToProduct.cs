namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmpresaToProduct : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "ID_Empresa", c => c.String(maxLength: 128));
            CreateIndex("dbo.Products", "ID_Empresa");
            AddForeignKey("dbo.Products", "ID_Empresa", "dbo.Companies", "userID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "ID_Empresa", "dbo.Companies");
            DropIndex("dbo.Products", new[] { "ID_Empresa" });
            AlterColumn("dbo.Products", "ID_Empresa", c => c.String());
        }
    }
}
