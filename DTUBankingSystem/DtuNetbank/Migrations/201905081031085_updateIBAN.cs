namespace DtuNetbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateIBAN : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BankAccounts", "Account_id", c => c.String());
            DropColumn("dbo.BankAccounts", "IBAN");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BankAccounts", "IBAN", c => c.String());
            DropColumn("dbo.BankAccounts", "Account_id");
        }
    }
}
