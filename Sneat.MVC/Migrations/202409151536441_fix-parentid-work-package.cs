namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixparentidworkpackage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkPackage", "WorkPackageID", c => c.Int());
            DropColumn("dbo.WorkPackage", "ƯorkPackageID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkPackage", "ƯorkPackageID", c => c.Int());
            DropColumn("dbo.WorkPackage", "WorkPackageID");
        }
    }
}
