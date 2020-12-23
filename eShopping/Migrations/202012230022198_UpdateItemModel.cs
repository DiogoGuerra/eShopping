namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateItemModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "UniPreco", c => c.Double(nullable: false));
            AddColumn("dbo.Items", "TotalPrice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "TotalPrice");
            DropColumn("dbo.Items", "UniPreco");
        }
    }
}
