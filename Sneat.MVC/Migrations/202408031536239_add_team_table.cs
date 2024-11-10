namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_team_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Team",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TechStack = c.String(),
                        Description = c.String(),
                        Status = c.Int(nullable: false),
                        IsDeleted = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Team");
        }
    }
}
