namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addprojectrole : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProject", "ProjectRole", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProject", "ProjectRole");
        }
    }
}
