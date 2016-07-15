namespace Domain.Repositories
{
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
            context.Adverts.AddRange(adverts);
            context.SaveChanges();
        }

        public void AddOrUpdateRange(IList<Advert> adverts)
        {
            var dBadverts = GetAll();
            List<Advert> newAdverts = adverts.Where(advert => dBadverts.All(x => x.No != advert.No)).ToList();
            AddRange(newAdverts);

            //List<Advert> oldAdverts = adverts.Except(newAdverts).ToList();
            //foreach (var oldAdvert in oldAdverts)
            //{
            //    var dBadvert = dBadverts.First(x => x.No == oldAdvert.No);
            //    if (dBadvert.Price != oldAdvert.Price || dBadvert.Description != oldAdvert.Description)
            //    {
            //        dBadvert = oldAdvert;
            //    }
            //}
            //context.SaveChanges();
        }
    }
}
