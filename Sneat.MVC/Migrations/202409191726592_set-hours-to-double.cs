namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sethourstodouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WorkPackage", "SpentTime", c => c.Double());
            AlterColumn("dbo.WorkPackage", "CompletePercent", c => c.Double());
            AlterColumn("dbo.TimeLog", "Hours", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TimeLog", "Hours", c => c.Int(nullable: false));
            AlterColumn("dbo.WorkPackage", "CompletePercent", c => c.Int());
            AlterColumn("dbo.WorkPackage", "SpentTime", c => c.Int());
        }
    }
}
