namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userdetailidentity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserDetail", "IdentityReceivedDate", c => c.DateTime());
            AddColumn("dbo.UserDetail", "IdentityReceivedPlace", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserDetail", "IdentityReceivedPlace");
            DropColumn("dbo.UserDetail", "IdentityReceivedDate");
        }
    }
}
