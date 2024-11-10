namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adduseravatar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Avatar", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "Avatar");
        }
    }
}
