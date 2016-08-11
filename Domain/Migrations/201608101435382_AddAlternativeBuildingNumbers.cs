namespace Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddAlternativeBuildingNumbers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlternativeBuildingNumbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        No = c.String(),
                        BuildingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Buildings", t => t.BuildingId)
                .Index(t => t.BuildingId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AlternativeBuildingNumbers", "BuildingId", "dbo.Buildings");
            DropIndex("dbo.AlternativeBuildingNumbers", new[] { "BuildingId" });
            DropTable("dbo.AlternativeBuildingNumbers");
        }
    }
}
