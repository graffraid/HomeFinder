namespace Web.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Web.Http;
    using Domain.Entities;
    using Domain.Repositories;
    using Infrastructure.Excrptions;

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

        [Route("buildings")]
        [HttpPost]
        public IHttpActionResult AddNewBuilding(Building building)
        {
            if (building == null || building.Street == string.Empty || building.ShortStreet == string.Empty || building.No == string.Empty)
            {
                throw new CustomHttpException(HttpStatusCode.BadRequest, "Incorrect data");
            }

            buildingRepository.AddNew(building);
            return Ok();
        }

        [Route("buildings/{id:int}")]
        [HttpDelete]
        public IHttpActionResult DeleteBuilding(int id)
        {
            buildingRepository.DeleteById(id);
            return Ok();
        }
    }
}
