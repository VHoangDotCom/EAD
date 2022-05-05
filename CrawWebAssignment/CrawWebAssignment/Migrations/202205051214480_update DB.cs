namespace CrawWebAssignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sources", "Article_Id", "dbo.Articles");
            DropIndex("dbo.Sources", new[] { "Article_Id" });
            DropPrimaryKey("dbo.Sources");
            AddColumn("dbo.Articles", "UrlSource", c => c.String());
            AddColumn("dbo.Articles", "Content", c => c.String());
            AddColumn("dbo.Articles", "CategoryId", c => c.String());
            AddColumn("dbo.Articles", "CreatedAt", c => c.Long(nullable: false));
            AddColumn("dbo.Sources", "SelectorSubUrl", c => c.String());
            AddColumn("dbo.Sources", "SelectorTitle", c => c.String());
            AddColumn("dbo.Sources", "SelectorImage", c => c.String());
            AddColumn("dbo.Sources", "SelectorDescrition", c => c.String());
            AddColumn("dbo.Sources", "SelectorContent", c => c.String());
            AddColumn("dbo.Sources", "CategoryId", c => c.String());
            AlterColumn("dbo.Sources", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Sources", "Id");
            DropColumn("dbo.Articles", "Url");
            DropColumn("dbo.Sources", "Name");
            DropColumn("dbo.Sources", "LinkSelector");
            DropColumn("dbo.Sources", "Article_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sources", "Article_Id", c => c.Int());
            AddColumn("dbo.Sources", "LinkSelector", c => c.String());
            AddColumn("dbo.Sources", "Name", c => c.String());
            AddColumn("dbo.Articles", "Url", c => c.String());
            DropPrimaryKey("dbo.Sources");
            AlterColumn("dbo.Sources", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Sources", "CategoryId");
            DropColumn("dbo.Sources", "SelectorContent");
            DropColumn("dbo.Sources", "SelectorDescrition");
            DropColumn("dbo.Sources", "SelectorImage");
            DropColumn("dbo.Sources", "SelectorTitle");
            DropColumn("dbo.Sources", "SelectorSubUrl");
            DropColumn("dbo.Articles", "CreatedAt");
            DropColumn("dbo.Articles", "CategoryId");
            DropColumn("dbo.Articles", "Content");
            DropColumn("dbo.Articles", "UrlSource");
            AddPrimaryKey("dbo.Sources", "Id");
            CreateIndex("dbo.Sources", "Article_Id");
            AddForeignKey("dbo.Sources", "Article_Id", "dbo.Articles", "Id");
        }
    }
}
