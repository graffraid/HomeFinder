using System.Web.Mvc;

namespace Web.Controllers
{
    using Domain.Repositories;

    public class HomeController : Controller
    {
        private readonly AdvertRepository advertRepository;

        public HomeController()
        {
            advertRepository = new AdvertRepository();
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
            ViewBag.Message = "Your application Buildings page.";

            return View();
        }

        public ActionResult Data()
        {
            ViewBag.Message = "Your Data page.";

            return View();
        }
    }
}