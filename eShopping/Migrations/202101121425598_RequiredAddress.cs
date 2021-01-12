namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredAddress : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Adress", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Adress", c => c.String());
        }
    }
}
