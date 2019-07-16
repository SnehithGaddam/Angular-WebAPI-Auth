using System.Web.Mvc;

namespace AngularWebApiAuthExample.WebApis.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
