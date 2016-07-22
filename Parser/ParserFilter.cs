namespace Parser
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repositories;

    using Infrastructure.Enums;

    public class ParserFilter
    {
        private readonly List<Advert> dbAdverts;

        public ParserFilter(List<Advert> dbAdverts)
        {
            this.dbAdverts = dbAdverts;
        }

        public List<Advert> GetNewAdverts(List<Advert> adverts)
        {
            var rezult = new List<Advert>();
            rezult = adverts.Where(advert => dbAdverts.All(x => x.No != advert.No)).ToList();
            foreach (var item in rezult)
            {
                item.Type = AdvertType.Actual;
            }
            return rezult;
        }

        public List<Advert> GetChangedAdverts(List<Advert> adverts)
        {
            var rezult = new List<Advert>();
            var existingAdverts = adverts.Where(advert => dbAdverts.Any(x => x.No == advert.No)).ToList();

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
                    || !dbAdvert.AdvertImages.Select(x => x.Url).ToList().SequenceEqual(existingAdvert.AdvertImages.Select(x => x.Url).ToList()))
                {
                    existingAdvert.InitialAdvert = dbAdvert;
                    existingAdvert.Type = AdvertType.Actual;
                    dbAdvert.Type = AdvertType.Changed;
                    rezult.Add(existingAdvert);
                }
            }
            return rezult;
        }

        public List<Advert> GetOutdatedAdverts(List<Advert> adverts)
        {
            var rezult = new List<Advert>();
            rezult = dbAdverts.Where(dbAdvert => adverts.All(advert => advert.No != dbAdvert.No)).ToList();
            foreach (var item in rezult)
            {
                item.Type = AdvertType.Outdated;
            }

            return rezult;
        }
    }
}
