using System.Web.Mvc;

namespace Web.Controllers
{
    using Domain.Repositories;

    public class HomeController : Controller
    {
        private readonly AdvertRepository advertRepository;
        private readonly BuildingRepository buildingRepository;

        public HomeController()
        {
            advertRepository = new AdvertRepository();
            buildingRepository = new BuildingRepository();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Adverts()
        {
            var adverts = advertRepository.GetAll();

            return View(adverts);
        }

        public ActionResult Buildings()
        {
            var buildings = buildingRepository.GetAll();

            return View(buildings);
        }

        public ActionResult Data()
        {
            return View();
        }
    }
}