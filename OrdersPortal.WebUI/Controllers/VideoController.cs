using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NLog;

using OrdersPortal.Application.Services;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.WebUI.Controllers
{
    public class VideoController : Controller
    {

	    private readonly IVideoContentService _videoContentService;
	    private readonly IVideoContentRepository _videoContentRepository;
	    private Logger _logger;

	    public VideoController(IVideoContentService videoContentService, IVideoContentRepository videoContentRepository)
	    {

		    _videoContentService = videoContentService;
		    _videoContentRepository = videoContentRepository;
		    _logger = LogManager.GetCurrentClassLogger();
		    

	    }


		// GET: Video
		public ActionResult Index()
        {
	        return RedirectToAction("List");
        }
        public ActionResult List()
        {
	        var list = _videoContentService.GetList();
	        return View(list);
        }

        public ActionResult Add()
        {
	        return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Add(VideoContent model)
        {
	        if (ModelState.IsValid)
	        {
		        try
		        {
			        _videoContentService.Add(model);
		        }
		        catch (Exception ex)
		        {
					_logger.Error(ex);
		        }
		        
		        return RedirectToAction("List");
			}

	        return View(model);

        }

        public ActionResult Edit(int id)
        {
	        VideoContent videoContent = _videoContentRepository.GetById(id);
	        return View(videoContent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VideoContent videoContent)
        {
	        try
	        {
		        _videoContentService.Edit(videoContent);
	        }
	        catch (Exception ex)
	        {
		        _logger.Error(ex);
			}

	        return RedirectToAction("List");
        }

        public ActionResult Remove(int id)
        {
	        try
	        {
		        _videoContentService.Remove(id);
	        }
	        catch (Exception ex)
	        {
		        _logger.Error(ex);
	        }

	        return RedirectToAction("List");

        }
	}
}