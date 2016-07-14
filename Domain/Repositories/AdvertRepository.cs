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
            return context.Adverts.ToList();
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
    }
}
