namespace DtuNetbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionsTearDown : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Transactions", "BankAccountId", "dbo.BankAccounts");
            DropIndex("dbo.Transactions", new[] { "BankAccountId" });
            DropPrimaryKey("dbo.BankAccounts");
            AlterColumn("dbo.BankAccounts", "Account_id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.BankAccounts", new[] { "Account_id", "UserId" });
            DropColumn("dbo.BankAccounts", "Id");
            DropColumn("dbo.BankAccounts", "AccountNumber");
            DropColumn("dbo.BankAccounts", "AccountName");
            DropColumn("dbo.BankAccounts", "Balance");
            DropTable("dbo.Transactions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        BankAccountId = c.Guid(nullable: false),
                        TransactionDate = c.DateTime(nullable: false),
                        AccountNumber = c.Long(nullable: false),
                        TransactionAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.BankAccounts", "Balance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BankAccounts", "AccountName", c => c.String());
            AddColumn("dbo.BankAccounts", "AccountNumber", c => c.String());
            AddColumn("dbo.BankAccounts", "Id", c => c.Guid(nullable: false));
            DropPrimaryKey("dbo.BankAccounts");
            AlterColumn("dbo.BankAccounts", "Account_id", c => c.String());
            AddPrimaryKey("dbo.BankAccounts", "Id");
            CreateIndex("dbo.Transactions", "BankAccountId");
            AddForeignKey("dbo.Transactions", "BankAccountId", "dbo.BankAccounts", "Id", cascadeDelete: true);
        }
    }
}
