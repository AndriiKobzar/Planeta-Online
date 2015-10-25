namespace Planeta_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTillTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventApplications", "From", c => c.DateTime(nullable: false));
            AddColumn("dbo.EventApplications", "Till", c => c.DateTime(nullable: false));
            AddColumn("dbo.Events", "From", c => c.DateTime(nullable: false));
            AddColumn("dbo.Events", "Till", c => c.DateTime(nullable: false));
            DropColumn("dbo.EventApplications", "Time");
            DropColumn("dbo.Events", "Time");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "Time", c => c.DateTime(nullable: false));
            AddColumn("dbo.EventApplications", "Time", c => c.DateTime(nullable: false));
            DropColumn("dbo.Events", "Till");
            DropColumn("dbo.Events", "From");
            DropColumn("dbo.EventApplications", "Till");
            DropColumn("dbo.EventApplications", "From");
        }
    }
}
