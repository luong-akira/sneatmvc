namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_user_workpackage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserWorkPackage",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AssignType = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        WorkPackageID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .ForeignKey("dbo.WorkPackage", t => t.WorkPackageID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.WorkPackageID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserWorkPackage", "WorkPackageID", "dbo.WorkPackage");
            DropForeignKey("dbo.UserWorkPackage", "UserID", "dbo.User");
            DropIndex("dbo.UserWorkPackage", new[] { "WorkPackageID" });
            DropIndex("dbo.UserWorkPackage", new[] { "UserID" });
            DropTable("dbo.UserWorkPackage");
        }
    }
}
