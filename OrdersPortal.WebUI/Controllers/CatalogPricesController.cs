using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NLog;

namespace OrdersPortal.WebUI.Controllers
{
    public class CatalogPricesController : Controller
    {

	    private readonly Logger _logger;
	   

	    public CatalogPricesController()
	    {
		
		    _logger = LogManager.GetCurrentClassLogger();
	    }	



		// GET: CatalogPrices
		public ActionResult Catalog2021(string id)
		{
			

            return View("Catalog2021",null,id );
        }
    }
}