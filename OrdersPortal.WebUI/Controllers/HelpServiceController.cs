using System.Web.Mvc;
using NLog;
using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Application.Services;
using OrdersPortal.Domain.Entities;


namespace OrdersPortal.WebUI.Controllers
{
    public class HelpServiceController : Controller
    {
        private readonly IHelpServiceService _helpServiceService;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();


        public HelpServiceController(IHelpServiceService helpServiceService)
        {
	       
	        _helpServiceService = helpServiceService;
        }
		// GET: HelpService
		public ActionResult CallManager(HelpServiceTypesEnum helpServiceType)
        {
            var result = _helpServiceService.CallManager(helpServiceType);
            return View(result);
        }
        public ActionResult Configure()
        {
            HelpServiceConfigureViewModel model = new HelpServiceConfigureViewModel();
            model = _helpServiceService.GetHelpServiceContacts();
            return View(model);
        }
        [HttpPost]
        public ActionResult Configure(HelpServiceConfigureViewModel model)
        {
	        _helpServiceService.SaveHelpServiceContacts(model);
            return RedirectToAction("Index","Home");
        }
    }
}