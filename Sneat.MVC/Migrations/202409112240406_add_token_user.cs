namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_token_user : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Token", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "Token");
        }
    }
}
