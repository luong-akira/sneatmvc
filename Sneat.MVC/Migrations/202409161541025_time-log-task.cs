namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class timelogtask : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeLog", "WorkPackageID", c => c.Int(nullable: false));
            CreateIndex("dbo.TimeLog", "WorkPackageID");
            AddForeignKey("dbo.TimeLog", "WorkPackageID", "dbo.WorkPackage", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimeLog", "WorkPackageID", "dbo.WorkPackage");
            DropIndex("dbo.TimeLog", new[] { "WorkPackageID" });
            DropColumn("dbo.TimeLog", "WorkPackageID");
        }
    }
}
