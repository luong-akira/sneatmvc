namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addworkpackagetable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkPackage",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Type = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        EstimateTime = c.Int(),
                        SpentTime = c.Int(),
                        RemainingTime = c.Int(),
                        CompletePercent = c.Int(),
                        PriorityPoint = c.Int(),
                        StartDate = c.DateTime(),
                        FinishDate = c.DateTime(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                        IsDeleted = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WorkPackage");
        }
    }
}
