namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addrelationshipuserteam : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserTeam",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TeamID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Team", t => t.TeamID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.TeamID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserTeam", "UserID", "dbo.User");
            DropForeignKey("dbo.UserTeam", "TeamID", "dbo.Team");
            DropIndex("dbo.UserTeam", new[] { "UserID" });
            DropIndex("dbo.UserTeam", new[] { "TeamID" });
            DropTable("dbo.UserTeam");
        }
    }
}
