using System.Web.Mvc;

namespace WebApiSample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "WebAPI 2.0";

            return View();
        }
    }
}
