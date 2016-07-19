using System.Web.Mvc;

namespace Web.Controllers
{
    using Domain.Repositories;

    public class HomeController : Controller
    {
        private AdvertRepository advertRepository;

        public HomeController()
        {
            advertRepository = new AdvertRepository();
        }

        public ActionResult Index()
        {
            var adverts = advertRepository.GetAll();

            return View(adverts);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}