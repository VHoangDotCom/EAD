namespace CrawWebAssignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SourceUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UrlSource = c.String(),
                        LinkSelector = c.String(),
                        UrlArticle = c.String(),
                        Title = c.String(),
                        Image = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Contents");
        }
    }
}
