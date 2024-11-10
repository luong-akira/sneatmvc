namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first_init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.District",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ProvinceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Province", t => t.ProvinceID, cascadeDelete: true)
                .Index(t => t.ProvinceID);
            
            CreateTable(
                "dbo.Province",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        Role = c.Int(nullable: false),
                        IsDeleted = c.Int(nullable: false),
                        Status = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                        DistrictID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.District", t => t.DistrictID)
                .Index(t => t.DistrictID);
            
            CreateTable(
                "dbo.Ward",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DistrictID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.District", t => t.DistrictID, cascadeDelete: true)
                .Index(t => t.DistrictID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ward", "DistrictID", "dbo.District");
            DropForeignKey("dbo.User", "DistrictID", "dbo.District");
            DropForeignKey("dbo.District", "ProvinceID", "dbo.Province");
            DropIndex("dbo.Ward", new[] { "DistrictID" });
            DropIndex("dbo.User", new[] { "DistrictID" });
            DropIndex("dbo.District", new[] { "ProvinceID" });
            DropTable("dbo.Ward");
            DropTable("dbo.User");
            DropTable("dbo.Province");
            DropTable("dbo.District");
        }
    }
}
