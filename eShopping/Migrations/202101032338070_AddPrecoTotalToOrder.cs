namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPrecoTotalToOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Preco_Total", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Preco_Total");
        }
    }
}
