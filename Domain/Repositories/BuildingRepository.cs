namespace Domain.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Entities;
    using Infrastructure.Excrptions;

    public class BuildingRepository
    {
        private FinderDbContext context;

        public BuildingRepository()
        {
            context = new FinderDbContext();
        }

        public List<Building> GetAll()
        {
            return context.Buildings.ToList();
        }

        public void AddNew(Building building)
        {
            context.Buildings.Add(building);
            context.SaveChanges();
        }

        public void DeleteById(int id)
        {
            var building = context.Buildings.FirstOrDefault(x => x.Id == id);
            if (building == null)
            {
                throw new CustomHttpException(HttpStatusCode.NotFound, "NotFound");
            }
            context.Buildings.Remove(building);
            context.SaveChanges();
        }
    }
}
