namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePriceToDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProductsOrders", "Preco_Produto", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductsOrders", "Preco_Produto", c => c.Int(nullable: false));
        }
    }
}
