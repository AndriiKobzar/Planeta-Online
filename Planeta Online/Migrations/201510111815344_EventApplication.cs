namespace Planeta_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventApplication : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventApplications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MyProperty = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Lecturer = c.String(),
                        Description = c.String(nullable: false),
                        Time = c.DateTime(nullable: false),
                        CreatorEmail = c.String(nullable: false),
                        CreatorName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EventApplications");
        }
    }
}
