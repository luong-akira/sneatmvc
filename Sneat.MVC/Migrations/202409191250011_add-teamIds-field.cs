namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addteamIdsfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "TeamIds", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Project", "TeamIds");
        }
    }
}
