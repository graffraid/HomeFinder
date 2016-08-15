namespace Domain.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Building
    {
        public int Id { get; set; }

        public string Street { get; set; }

        public string ShortStreet { get; set; }

        public string No { get; set; }

        [NotMapped]
        public string Address => $"{Street}, {No}";

        public DbGeography Coordinate { get; set; }

        [NotMapped]
        public double Latitude => Coordinate.Latitude ?? 0;

        [NotMapped]
        public double Longitude => Coordinate.Longitude ?? 0;

        public virtual List<AlternativeBuildingNumber> AlternativeBuildingNumbers { get; set; }
        
        public virtual List<Advert> Adverts { get; set; }
    }
}
