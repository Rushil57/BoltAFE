using System.Web.Mvc;

namespace BoltAFE.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}