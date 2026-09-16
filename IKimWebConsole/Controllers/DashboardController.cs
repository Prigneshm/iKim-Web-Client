using IKimWebConsole.Filter;
using System.Web.Mvc;

namespace IKimWebConsole.Controllers
{
    [Authentication]
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}