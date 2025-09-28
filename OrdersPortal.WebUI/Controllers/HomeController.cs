using System.Web.Mvc;

namespace OrdersPortal.WebUI.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}