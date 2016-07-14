namespace Domain.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Entities;

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
    }
}
