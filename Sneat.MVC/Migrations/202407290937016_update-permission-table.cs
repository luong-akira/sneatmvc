namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatepermissiontable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Permission", "ParentID", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Permission", "ParentID", c => c.Int(nullable: false));
        }
    }
}
