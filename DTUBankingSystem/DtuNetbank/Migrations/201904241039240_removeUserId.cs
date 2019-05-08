namespace DtuNetbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeUserId : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.BankAccounts", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BankAccounts", "UserId", c => c.Guid(nullable: false));
        }
    }
}
