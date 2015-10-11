namespace Planeta_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAttachmnts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventApplications", "Attachments", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventApplications", "Attachments");
        }
    }
}
