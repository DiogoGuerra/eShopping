namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCartModel1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.ProductsOrders", "Cart_ID", c => c.Int());
            CreateIndex("dbo.ProductsOrders", "Cart_ID");
            AddForeignKey("dbo.ProductsOrders", "Cart_ID", "dbo.Carts", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductsOrders", "Cart_ID", "dbo.Carts");
            DropIndex("dbo.ProductsOrders", new[] { "Cart_ID" });
            DropColumn("dbo.ProductsOrders", "Cart_ID");
            DropTable("dbo.Carts");
        }
    }
}
