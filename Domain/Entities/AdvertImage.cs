namespace Domain.Entities
{
    public class AdvertImage
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string CachedUrl { get; set; }

        public int AdvertId { get; set; }

        public virtual Advert Advert { get; set; }
    }
}
