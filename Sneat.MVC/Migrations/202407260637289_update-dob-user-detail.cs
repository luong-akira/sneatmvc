namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedobuserdetail : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserDetail", "DateOfBirth", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserDetail", "DateOfBirth", c => c.DateTime(nullable: false));
        }
    }
}
