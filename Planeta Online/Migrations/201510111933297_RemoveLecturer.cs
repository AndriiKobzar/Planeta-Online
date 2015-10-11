namespace Planeta_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveLecturer : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.EventApplications", "Lecturer");
            DropColumn("dbo.Events", "Lecturer");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "Lecturer", c => c.String());
            AddColumn("dbo.EventApplications", "Lecturer", c => c.String());
        }
    }
}
