namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addpermissionicon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Permission", "TabIcon", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Permission", "TabIcon");
        }
    }
}
