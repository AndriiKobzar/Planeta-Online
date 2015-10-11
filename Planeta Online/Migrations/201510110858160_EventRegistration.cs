namespace Planeta_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventRegistration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventRegistrations", "VisitorName", c => c.String());
            AddColumn("dbo.EventRegistrations", "VisitorEmail", c => c.String());
            DropColumn("dbo.EventRegistrations", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EventRegistrations", "UserId", c => c.String());
            DropColumn("dbo.EventRegistrations", "VisitorEmail");
            DropColumn("dbo.EventRegistrations", "VisitorName");
        }
    }
}
