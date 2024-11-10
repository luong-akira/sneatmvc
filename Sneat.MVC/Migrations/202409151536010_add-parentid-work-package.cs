namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addparentidworkpackage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkPackage", "ƯorkPackageID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkPackage", "ƯorkPackageID");
        }
    }
}
