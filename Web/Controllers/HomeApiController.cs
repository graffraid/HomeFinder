namespace Web.Controllers
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Hosting;
    using System.Web.Http;
    using Domain.Entities;
    using Domain.Repositories;
    using Infrastructure.Excrptions;

    using Parser;

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

        [Route("data/adverts")]
        [HttpGet]
        public IHttpActionResult UpdateAdverts()
        {
            var urlToParse ="https://www.avito.ru/voronezh/kvartiry/prodam/vtorichka/kirpichnyy_dom?district=150&f=549_5698-5699-5700"; //ToDo: pass it as parameter.
            var hubUrl = $"{Request.RequestUri.Scheme}://{Request.RequestUri.Host}";
            var picturesPath = ConfigurationManager.AppSettings["images:path"];
            var absolutePicturesPath = HostingEnvironment.MapPath($"~/{ConfigurationManager.AppSettings["images:path"]}");

            var avitoParser = new Parser(urlToParse);
            Task.Run(() => avitoParser.Parse(hubUrl, picturesPath, absolutePicturesPath));
            return Ok();
        }
    }
}
