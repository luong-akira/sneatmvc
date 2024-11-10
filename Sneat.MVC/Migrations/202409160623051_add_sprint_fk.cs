namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_sprint_fk : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.WorkPackage", "SprintID");
            AddForeignKey("dbo.WorkPackage", "SprintID", "dbo.Sprint", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkPackage", "SprintID", "dbo.Sprint");
            DropIndex("dbo.WorkPackage", new[] { "SprintID" });
        }
    }
}
