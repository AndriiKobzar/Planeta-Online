namespace Planeta_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePoster : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventPosters", "EventId", c => c.Int(nullable: false));
            DropColumn("dbo.EventPosters", "CreatorEmail");
            DropColumn("dbo.EventPosters", "CreatorName");
            DropColumn("dbo.EventPosters", "CreatorPhone");
            DropColumn("dbo.EventPosters", "Name");
            DropColumn("dbo.EventPosters", "Description");
            DropColumn("dbo.EventPosters", "From");
            DropColumn("dbo.EventPosters", "Till");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EventPosters", "Till", c => c.DateTime(nullable: false));
            AddColumn("dbo.EventPosters", "From", c => c.DateTime(nullable: false));
            AddColumn("dbo.EventPosters", "Description", c => c.String());
            AddColumn("dbo.EventPosters", "Name", c => c.String());
            AddColumn("dbo.EventPosters", "CreatorPhone", c => c.String(nullable: false));
            AddColumn("dbo.EventPosters", "CreatorName", c => c.String(nullable: false));
            AddColumn("dbo.EventPosters", "CreatorEmail", c => c.String(nullable: false));
            DropColumn("dbo.EventPosters", "EventId");
        }
    }
}
