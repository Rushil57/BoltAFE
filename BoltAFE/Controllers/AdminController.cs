using System.Web.Mvc;

namespace BoltAFE.Controllers
{
    public class AdminController :  BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}