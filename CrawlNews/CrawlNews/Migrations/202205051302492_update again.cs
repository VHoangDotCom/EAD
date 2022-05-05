namespace CrawlNews.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateagain : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Sourses");
            AlterColumn("dbo.Sourses", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Sourses", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Sourses");
            AlterColumn("dbo.Sourses", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Sourses", "Id");
        }
    }
}
