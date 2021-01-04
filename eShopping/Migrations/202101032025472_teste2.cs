namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class teste2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ID_Empresa", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "ID_Empresa");
        }
    }
}
