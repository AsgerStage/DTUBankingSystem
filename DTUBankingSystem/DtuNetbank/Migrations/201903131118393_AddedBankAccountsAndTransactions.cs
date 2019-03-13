namespace DtuNetbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBankAccountsAndTransactions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BankAccounts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        AccountNumber = c.String(),
                        AccountName = c.String(),
                        IBAN = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        BankAccountId = c.Guid(nullable: false),
                        TransactionDate = c.DateTime(nullable: false),
                        AccountNumber = c.Long(nullable: false),
                        TransactionAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BankAccounts", t => t.BankAccountId, cascadeDelete: true)
                .Index(t => t.BankAccountId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "BankAccountId", "dbo.BankAccounts");
            DropForeignKey("dbo.BankAccounts", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Transactions", new[] { "BankAccountId" });
            DropIndex("dbo.BankAccounts", new[] { "User_Id" });
            DropTable("dbo.Transactions");
            DropTable("dbo.BankAccounts");
        }
    }
}
