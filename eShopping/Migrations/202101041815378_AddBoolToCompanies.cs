namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBoolToCompanies : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "EstaEliminado", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "EstaEliminado");
        }
    }
}
