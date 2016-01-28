namespace Planeta_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BlogMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BlogPosts", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.BlogPosts", "Text", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BlogPosts", "Text", c => c.String());
            AlterColumn("dbo.BlogPosts", "Title", c => c.String());
        }
    }
}
