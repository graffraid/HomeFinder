namespace Domain.Migrations
{
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
                .ForeignKey("dbo.Adverts", t => t.AdvertId)
                .Index(t => t.AdvertId);
            
            CreateTable(
                "dbo.Adverts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        No = c.Int(),
                        PlacementDate = c.DateTime(),
                        Price = c.Int(),
                        RoomCount = c.Int(),
                        Space = c.Double(),
                        Floor = c.Int(),
                        TotalFloor = c.Int(),
                        Description = c.String(),
                        SellerName = c.String(),
                        SellerPhone = c.String(),
                        IsSellerAgency = c.Boolean(nullable: false),
                        AddDate = c.DateTime(nullable: false),
                        BuildingId = c.Int(nullable: false),
                        InitialAdvert_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Buildings", t => t.BuildingId)
                .ForeignKey("dbo.Adverts", t => t.InitialAdvert_Id)
                .Index(t => t.BuildingId)
                .Index(t => t.InitialAdvert_Id);
            
            CreateTable(
                "dbo.Buildings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Street = c.String(),
                        ShortStreet = c.String(),
                        No = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AdvertImages", "AdvertId", "dbo.Adverts");
            DropForeignKey("dbo.Adverts", "InitialAdvert_Id", "dbo.Adverts");
            DropForeignKey("dbo.Adverts", "BuildingId", "dbo.Buildings");
            DropIndex("dbo.Adverts", new[] { "InitialAdvert_Id" });
            DropIndex("dbo.Adverts", new[] { "BuildingId" });
            DropIndex("dbo.AdvertImages", new[] { "AdvertId" });
            DropTable("dbo.Buildings");
            DropTable("dbo.Adverts");
            DropTable("dbo.AdvertImages");
        }
    }
}
