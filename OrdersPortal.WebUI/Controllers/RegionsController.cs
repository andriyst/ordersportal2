using System;
using System.Web.Mvc;
using NLog;
using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Application.Services;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;


namespace OrdersPortal.WebUI.Controllers
{
    public class RegionsController : Controller
    {
	    private readonly IRegionRepository _regionRepository;
	    private readonly IRegionService _regionService;
	    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();


	    public RegionsController(IRegionRepository regionRepository, IRegionService regionService)
	    {
		    _regionRepository = regionRepository;
		    _regionService = regionService;
	    }
		//
		// GET: /Regions/

		public ActionResult Index()
		{
			return RedirectToAction("List");
		}

		// GET: /Regions/RegionList
		
		public ActionResult List()
		{
			var model = _regionService.GetRegionList();
			return View(model);
		}


		// GET: /Regions/RegionAdd

		public ActionResult Add()
		{
			return View();
		}

		// POST: /Regions/RegionAdd
		[HttpPost]
		[ValidateAntiForgeryToken]
	
		public ActionResult Add(RegionAddViewModel model)
		{
			if (ModelState.IsValid)
			{
				Region region = new Region
				{
					RegionName = model.RegionName

				};
				_regionService.AddRegion(region);
		
			}

			return RedirectToAction("List");

		}

		// GET: /Regions/RegionEdit		
		public ActionResult Edit(int regionId)
		{			
			Region region = _regionRepository.GetById(regionId);
			return View(region);
		}

		//
		// POST: /Regions/EditRegion
		[HttpPost]
		[ValidateAntiForgeryToken]		
		public ActionResult Edit(Region region)
		{
			_regionService.EditRegion(region);

			return RedirectToAction("List");
		}

		public ActionResult Del(int regionId)
		{
			try
			{
				_regionService.RemoveRegion(regionId);
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
			}

			return RedirectToAction("List");

		}

	}
}