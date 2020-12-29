namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ADDStringIDToCompany : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "Email", c => c.String());
            AddColumn("dbo.Companies", "userID", c => c.String());
            AlterColumn("dbo.Companies", "Nome", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Companies", "Nome", c => c.String());
            DropColumn("dbo.Companies", "userID");
            DropColumn("dbo.Companies", "Email");
        }
    }
}
