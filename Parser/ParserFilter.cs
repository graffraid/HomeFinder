namespace Parser
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repositories;

    public class ParserFilter
    {
        private readonly AdvertRepository advertRepository;

        public ParserFilter(AdvertRepository advertRepository)
        {
            this.advertRepository = advertRepository;
        }

        public List<Advert> GetNewAdverts(List<Advert> advertElements)
        {
            var dbAdverts = advertRepository.GetAll();
            return advertElements.Where(advertElement => dbAdverts.All(x => x.No != advertElement.No)).ToList();
        }
        public List<Advert> GetChangedAdverts(List<Advert> advertElements)
        {
            var rezult = new List<Advert>();
            var dbAdverts = advertRepository.GetAll();
            var existingAdverts = advertElements.Where(advertElement => dbAdverts.Any(x => x.No == advertElement.No)).ToList();

            foreach (var existingAdvert in existingAdverts)
            {
                var dbAdvert = dbAdverts.First(x => x.No == existingAdvert.No);

                if (dbAdvert.Price != existingAdvert.Price
                    || dbAdvert.Description != existingAdvert.Description
                    || dbAdvert.Floor != existingAdvert.Floor
                    || dbAdvert.TotalFloor != existingAdvert.TotalFloor
                    || dbAdvert.RoomCount != existingAdvert.RoomCount
                    || dbAdvert.Space != existingAdvert.Space
                    || dbAdvert.SellerName != existingAdvert.SellerName
                    || dbAdvert.SellerPhone != existingAdvert.SellerPhone
                    || dbAdvert.IsSellerAgency != existingAdvert.IsSellerAgency
                    || !dbAdvert.AdvertImages.Select(x => x.Url).ToList().Equals(existingAdvert.AdvertImages.Select(x => x.Url).ToList()))
                {
                    existingAdvert.InitialAdvert = dbAdvert;
                    rezult.Add(existingAdvert);
                }
            }
            return rezult;
        }
    }
}
