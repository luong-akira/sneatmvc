namespace Sneat.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_user_detail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserDetail", "BankID", c => c.Int());
            AddColumn("dbo.UserDetail", "BankAccountName", c => c.String());
            AddColumn("dbo.UserDetail", "BankAccountNo", c => c.String());
            CreateIndex("dbo.UserDetail", "BankID");
            AddForeignKey("dbo.UserDetail", "BankID", "dbo.Bank", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDetail", "BankID", "dbo.Bank");
            DropIndex("dbo.UserDetail", new[] { "BankID" });
            DropColumn("dbo.UserDetail", "BankAccountNo");
            DropColumn("dbo.UserDetail", "BankAccountName");
            DropColumn("dbo.UserDetail", "BankID");
        }
    }
}
