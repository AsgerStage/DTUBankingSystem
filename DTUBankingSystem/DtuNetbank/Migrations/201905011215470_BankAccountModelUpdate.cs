namespace DtuNetbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BankAccountModelUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BankAccounts", "UserId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BankAccounts", "UserId");
        }
    }
}
