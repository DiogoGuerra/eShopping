namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTaxaPromocaoToProducts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Taxa_Promocao", c => c.Double(nullable: true));
           
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Taxa_Promocao");
        }
    }
}
