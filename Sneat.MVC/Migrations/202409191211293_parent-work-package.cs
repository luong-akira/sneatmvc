namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class parentworkpackage : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.WorkPackage", "WorkPackageID");
            AddForeignKey("dbo.WorkPackage", "WorkPackageID", "dbo.WorkPackage", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkPackage", "WorkPackageID", "dbo.WorkPackage");
            DropIndex("dbo.WorkPackage", new[] { "WorkPackageID" });
        }
    }
}
