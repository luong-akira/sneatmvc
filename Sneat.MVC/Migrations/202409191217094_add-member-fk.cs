namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addmemberfk : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.TimeLog", "MemberID");
            AddForeignKey("dbo.TimeLog", "MemberID", "dbo.User", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimeLog", "MemberID", "dbo.User");
            DropIndex("dbo.TimeLog", new[] { "MemberID" });
        }
    }
}
