namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addrelationshipsprinttask : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkPackage", "SprintID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkPackage", "SprintID");
        }
    }
}
