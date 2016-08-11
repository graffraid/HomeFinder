namespace Domain.Entities
{
    using System.Collections.Generic;

    public class Building
    {
        public int Id { get; set; }

        public string Street { get; set; }

        public string ShortStreet { get; set; }

        public string No { get; set; }

        public virtual List<AlternativeBuildingNumber> AlternativeBuildingNumbers { get; set; }
        
        public virtual List<Advert> Adverts { get; set; }
    }
}
