namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userdetailtable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.User", "DistrictID", "dbo.District");
            DropIndex("dbo.User", new[] { "DistrictID" });
            CreateTable(
                "dbo.UserDetail",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        Gender = c.Int(nullable: false),
                        Identity = c.String(),
                        IdentityImages = c.String(),
                        DistrictHomeID = c.Int(),
                        HomeAddress = c.String(),
                        DistrictOfficeID = c.Int(),
                        OfficeAddress = c.String(),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.District", t => t.DistrictHomeID)
                .ForeignKey("dbo.District", t => t.DistrictOfficeID)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.DistrictHomeID)
                .Index(t => t.DistrictOfficeID)
                .Index(t => t.UserID);
            
            DropColumn("dbo.User", "Role");
            DropColumn("dbo.User", "DistrictID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "DistrictID", c => c.Int());
            AddColumn("dbo.User", "Role", c => c.Int(nullable: false));
            DropForeignKey("dbo.UserDetail", "UserID", "dbo.User");
            DropForeignKey("dbo.UserDetail", "DistrictOfficeID", "dbo.District");
            DropForeignKey("dbo.UserDetail", "DistrictHomeID", "dbo.District");
            DropIndex("dbo.UserDetail", new[] { "UserID" });
            DropIndex("dbo.UserDetail", new[] { "DistrictOfficeID" });
            DropIndex("dbo.UserDetail", new[] { "DistrictHomeID" });
            DropTable("dbo.UserDetail");
            CreateIndex("dbo.User", "DistrictID");
            AddForeignKey("dbo.User", "DistrictID", "dbo.District", "ID");
        }
    }
}
