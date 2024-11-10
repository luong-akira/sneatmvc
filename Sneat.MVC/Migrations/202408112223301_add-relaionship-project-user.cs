namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addrelaionshipprojectuser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProject",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Project", t => t.ProjectID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.ProjectID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProject", "UserID", "dbo.User");
            DropForeignKey("dbo.UserProject", "ProjectID", "dbo.Project");
            DropIndex("dbo.UserProject", new[] { "UserID" });
            DropIndex("dbo.UserProject", new[] { "ProjectID" });
            DropTable("dbo.UserProject");
        }
    }
}
