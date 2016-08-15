namespace Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddCoordinateForBuilding : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Buildings", "Coordinate", c => c.Geography());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Buildings", "Coordinate");
        }
    }
}
