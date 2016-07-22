namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;

    public class Advert
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public int? No { get; set; }

        public DateTime? PlacementDate { get; set; }

        public int? Price { get; set; }

        public int? RoomCount { get; set; }

        public double? Space { get; set; }

        public int? Floor { get; set; }

        public int? TotalFloor { get; set; }

        public string Description { get; set; }

        public string SellerName { get; set; }

        public string SellerPhone { get; set; }

        public bool IsSellerAgency { get; set; }

        public DateTime AddDate { get; set; }

        public int BuildingId { get; set; }

        public virtual Building Building { get; set; }

        public virtual List<AdvertImage> AdvertImages { get; set; }

        public virtual Advert InitialAdvert { get; set; }

        public virtual Advert ChangedAdvert { get; set; }
    }
}
