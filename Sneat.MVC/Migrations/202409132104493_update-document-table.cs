namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedocumenttable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Document", "IsDeleted", c => c.Int(nullable: false));
            AddColumn("dbo.Document", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Document", "UpdatedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Document", "UpdatedDate");
            DropColumn("dbo.Document", "CreatedDate");
            DropColumn("dbo.Document", "IsDeleted");
        }
    }
}
