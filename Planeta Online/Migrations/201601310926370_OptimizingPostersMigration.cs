namespace Planeta_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptimizingPostersMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "PosterPath", c => c.String());
            DropTable("dbo.EventPosters");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.EventPosters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventId = c.Int(nullable: false),
                        PosterPath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Events", "PosterPath");
        }
    }
}
