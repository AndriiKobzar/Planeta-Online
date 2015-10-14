namespace Planeta_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Renaming : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.EventApplications", "MyProperty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EventApplications", "MyProperty", c => c.Int(nullable: false));
        }
    }
}
