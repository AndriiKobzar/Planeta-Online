namespace Planeta_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostersMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Event_Id", "dbo.Events");
            DropIndex("dbo.AspNetUsers", new[] { "Event_Id" });
            CreateTable(
                "dbo.EventPosters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatorEmail = c.String(nullable: false),
                        CreatorName = c.String(nullable: false),
                        CreatorPhone = c.String(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        From = c.DateTime(nullable: false),
                        Till = c.DateTime(nullable: false),
                        PosterPath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Events", "Attachments");
            DropColumn("dbo.AspNetUsers", "Event_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Event_Id", c => c.Int());
            AddColumn("dbo.Events", "Attachments", c => c.String());
            DropTable("dbo.EventPosters");
            CreateIndex("dbo.AspNetUsers", "Event_Id");
            AddForeignKey("dbo.AspNetUsers", "Event_Id", "dbo.Events", "Id");
        }
    }
}
