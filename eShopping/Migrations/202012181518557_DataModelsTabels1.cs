namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataModelsTabels1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Preco_Produto", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Preco_Produto");
        }
    }
}
