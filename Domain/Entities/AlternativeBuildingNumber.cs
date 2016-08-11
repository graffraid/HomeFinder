namespace Domain.Entities
{
    public class AlternativeBuildingNumber
    {
        public int Id { get; set; }

        public string No { get; set; }

        public int BuildingId { get; set; }

        public virtual Building Building { get; set; }
    }
}
