namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BoolEstaEliminado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "EstaEliminado", c => c.Boolean(nullable: false));
            AddColumn("dbo.Products", "EstaEliminado", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "EstaEliminado");
            DropColumn("dbo.Categories", "EstaEliminado");
        }
    }
}
