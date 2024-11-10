namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addqrurluserdetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserDetail", "BankQRImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserDetail", "BankQRImage");
        }
    }
}
