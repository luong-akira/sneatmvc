namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class timelogtaskrelation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeLog", "Sprint_ID", c => c.Int());
            CreateIndex("dbo.TimeLog", "Sprint_ID");
            AddForeignKey("dbo.TimeLog", "Sprint_ID", "dbo.Sprint", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimeLog", "Sprint_ID", "dbo.Sprint");
            DropIndex("dbo.TimeLog", new[] { "Sprint_ID" });
            DropColumn("dbo.TimeLog", "Sprint_ID");
        }
    }
}
