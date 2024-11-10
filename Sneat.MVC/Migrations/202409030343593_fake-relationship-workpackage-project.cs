namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fakerelationshipworkpackageproject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkPackage", "ProjectID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkPackage", "ProjectID");
        }
    }
}
