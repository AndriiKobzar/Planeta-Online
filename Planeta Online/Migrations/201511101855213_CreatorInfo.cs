namespace Planeta_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatorInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventApplications", "CreatorPhone", c => c.String(nullable: false));
            AddColumn("dbo.Events", "CreatorEmail", c => c.String(nullable: false));
            AddColumn("dbo.Events", "CreatorName", c => c.String(nullable: false));
            AddColumn("dbo.Events", "CreatorPhone", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "CreatorPhone");
            DropColumn("dbo.Events", "CreatorName");
            DropColumn("dbo.Events", "CreatorEmail");
            DropColumn("dbo.EventApplications", "CreatorPhone");
        }
    }
}
