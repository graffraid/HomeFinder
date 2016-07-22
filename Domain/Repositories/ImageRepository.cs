namespace Domain.Repositories
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Entities;

    public class ImageRepository
    {
        private FinderDbContext context;

        public ImageRepository()
        {
            context = new FinderDbContext();
        }

        public List<AdvertImage> GetAllUncached()
        {
            return context.AdvertImages.Where(img => img.CachedUrl == null).ToList();
        }

        public virtual void EditRange(IList<AdvertImage> images)
        {
            foreach (var image in images)
            {
                context.Entry(image).State = EntityState.Modified;
            }
            context.SaveChanges();
        }
    }
}
