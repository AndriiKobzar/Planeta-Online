namespace Planeta_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Attachments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Attachments", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "Attachments");
        }
    }
}
