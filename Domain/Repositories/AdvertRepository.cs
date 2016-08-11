namespace Domain.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Entities;

    public class AdvertRepository
    {
        private FinderDbContext context;

        public AdvertRepository()
        {
            context = new FinderDbContext();
        }

        public Advert Get(int id)
        {
            return context.Adverts.Include("AdvertImages")
                                  .Include("Building")
                                  .Include("InitialAdvert")
                                  .Include("ChangedAdvert")
                                  .FirstOrDefault(adv => adv.Id == id);
        }


        public List<Advert> GetAll()
        {
            return context.Adverts.Include("AdvertImages")
                                  .Include("Building")
                                  .Include("InitialAdvert")
                                  .Include("ChangedAdvert")
                                  .OrderBy(x => x.Type)
                                  .ThenByDescending(x => x.AddDate)
                                  .Where(x => x.ChangedAdvert == null)
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

        public virtual void EditRange(IList<Advert> adverts)
        {
            foreach (var advert in adverts)
            {
                context.Entry(advert).State = EntityState.Modified;
            }
            context.SaveChanges();
        }
    }
}
