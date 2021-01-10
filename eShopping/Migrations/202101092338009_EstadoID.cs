namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EstadoID : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "Estado_ID", "dbo.Status");
            DropIndex("dbo.Orders", new[] { "Estado_ID" });
            RenameColumn(table: "dbo.Orders", name: "Estado_ID", newName: "EstadoID");
            AlterColumn("dbo.Orders", "EstadoID", c => c.Int(nullable: false));
            CreateIndex("dbo.Orders", "EstadoID");
            AddForeignKey("dbo.Orders", "EstadoID", "dbo.Status", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "EstadoID", "dbo.Status");
            DropIndex("dbo.Orders", new[] { "EstadoID" });
            AlterColumn("dbo.Orders", "EstadoID", c => c.Int());
            RenameColumn(table: "dbo.Orders", name: "EstadoID", newName: "Estado_ID");
            CreateIndex("dbo.Orders", "Estado_ID");
            AddForeignKey("dbo.Orders", "Estado_ID", "dbo.Status", "ID");
        }
    }
}
