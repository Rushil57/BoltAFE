using System.Web.Mvc;

namespace BoltAFE.Controllers
{
    public class DashboardController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}