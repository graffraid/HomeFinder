namespace Web.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Domain.Entities;
    using Domain.Repositories;

    [RoutePrefix("api/home")]
    public class HomeApiController : ApiController
    {
        private readonly BuildingRepository buildingRepository;

        public HomeApiController()
        {
            buildingRepository = new BuildingRepository();
        }

        [Route("buildings")]
        [HttpGet]
        public List<Building> GetAllBuildings()
        {
            return buildingRepository.GetAll();
        }
    }
}
