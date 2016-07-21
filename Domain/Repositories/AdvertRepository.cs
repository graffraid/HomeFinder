namespace Domain.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Entities;

    public class AdvertRepository
    {
        private FinderDbContext context;

        public AdvertRepository()
        {
            context = new FinderDbContext();
        }

        public List<Advert> GetAll()
        {
            return context.Adverts.Include("AdvertImages").Include("Building").ToList();
        }

        public void Add(Advert advert)
        {
            context.Adverts.Add(advert);
            context.SaveChanges();
        }

        public void AddRange(IList<Advert> adverts)
        {
            foreach (var advert in adverts)
            {
                advert.AddDate = DateTime.Now;
                advert.UpdateDate = null;
            }

            context.Adverts.AddRange(adverts);
            context.SaveChanges();
        }

        //ToDo: Test this method!
        public void AddOrUpdateRange(IList<Advert> adverts)
        {
            var dbAadverts = GetAll();
            List<Advert> newAdverts = adverts.Where(advert => dbAadverts.All(x => x.No != advert.No)).ToList();
            AddRange(newAdverts);

            List<Advert> existingAdverts = adverts.Except(newAdverts).ToList();
            foreach (var existingAdvert in existingAdverts)
            {
                var dbAdvert = dbAadverts.First(x => x.No == existingAdvert.No);
                if (dbAdvert.Price != existingAdvert.Price)
                {
                    dbAdvert.Price = existingAdvert.Price;
                }
                if (dbAdvert.Description != existingAdvert.Description)
                {
                    dbAdvert.Description = existingAdvert.Description;
                }
                if (dbAdvert.Floor != existingAdvert.Floor)
                {
                    dbAdvert.Floor = existingAdvert.Floor;
                }
                if (dbAdvert.TotalFloor != existingAdvert.TotalFloor)
                {
                    dbAdvert.TotalFloor = existingAdvert.TotalFloor;
                }
                if (dbAdvert.RoomCount != existingAdvert.RoomCount)
                {
                    dbAdvert.RoomCount = existingAdvert.RoomCount;
                }
                if (dbAdvert.Space != existingAdvert.Space)
                {
                    dbAdvert.Space = existingAdvert.Space;
                }
                if (dbAdvert.SellerName != existingAdvert.SellerName)
                {
                    dbAdvert.SellerName = existingAdvert.SellerName;
                }
                if (dbAdvert.SellerPhone != existingAdvert.SellerPhone)
                {
                    dbAdvert.SellerPhone = existingAdvert.SellerPhone;
                }
                if (dbAdvert.IsSellerAgency != existingAdvert.IsSellerAgency)
                {
                    dbAdvert.IsSellerAgency = existingAdvert.IsSellerAgency;
                }
                if (!dbAdvert.AdvertImages.Select(x => x.Url).ToList().Equals(existingAdvert.AdvertImages.Select(x => x.Url).ToList()))
                {
                    var newUrls = existingAdvert.AdvertImages.Select(x => x.Url).ToList().Where(url => dbAdvert.AdvertImages.Select(x => x.Url).ToList().All(x => x != url)).ToList();
                    foreach (var newUrl in newUrls)
                    {
                        dbAdvert.AdvertImages.Add(new AdvertImage
                                                      {
                                                          Url = newUrl
                                                      });
                    }
                }
            }
            context.SaveChanges();
        }
    }
}
