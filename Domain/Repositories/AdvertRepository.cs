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
            return context.Adverts.Include("AdvertImages")
                                  .Include("Building")
                                  .OrderByDescending(x => x.AddDate)
                                  .ToList();
        }

        public void Add(Advert advert)
        {
            context.Adverts.Add(advert);
            context.SaveChanges();
        }

        public void AddRange(IList<Advert> adverts)
        {
            if (adverts != null && adverts.Count > 0)
            {
                foreach (var advert in adverts)
                {
                    advert.AddDate = DateTime.Now;
                }

                context.Adverts.AddRange(adverts);
                context.SaveChanges();
            }
        }
    }
}
