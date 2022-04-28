namespace BankService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bankservice : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        Password = c.String(),
                        Balancer = c.String(),
                        AccountNumber = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TransactionHistories",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Amount = c.Double(nullable: false),
                        SenderAccountNumber = c.String(),
                        ReceiverAccountNumber = c.String(),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TransactionHistories");
            DropTable("dbo.Accounts");
        }
    }
}
