namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialiseMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdvertImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        CachedUrl = c.String(),
                        AdvertId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Adverts", t => t.AdvertId, cascadeDelete: true)
                .Index(t => t.AdvertId);
            
            CreateTable(
                "dbo.Adverts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        No = c.Int(nullable: false),
                        PlacementDate = c.DateTime(nullable: false),
                        Price = c.Int(nullable: false),
                        RoomCount = c.Int(nullable: false),
                        Space = c.Int(nullable: false),
                        Floor = c.Int(nullable: false),
                        TotalFloor = c.Int(nullable: false),
                        Description = c.String(),
                        SellerName = c.String(),
                        SellerPhone = c.String(),
                        BuildingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Buildings", t => t.BuildingId, cascadeDelete: true)
                .Index(t => t.BuildingId);
            
            CreateTable(
                "dbo.Buildings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Street = c.String(),
                        No = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Adverts", "BuildingId", "dbo.Buildings");
            DropForeignKey("dbo.AdvertImages", "AdvertId", "dbo.Adverts");
            DropIndex("dbo.Adverts", new[] { "BuildingId" });
            DropIndex("dbo.AdvertImages", new[] { "AdvertId" });
            DropTable("dbo.Buildings");
            DropTable("dbo.Adverts");
            DropTable("dbo.AdvertImages");
        }
    }
}
