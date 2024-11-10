namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtimelogtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TimeLog",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Hours = c.Int(nullable: false),
                        TotalWorkingTime = c.Int(nullable: false),
                        MemberID = c.Int(nullable: false),
                        CreatedByID = c.Int(nullable: false),
                        IsDeleted = c.Int(nullable: false),
                        LogDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TimeLog");
        }
    }
}
