using System;
using System.Web.Mvc;
using NLog;

using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Application.Services;

namespace OrdersPortal.WebUI.Controllers
{
    public class FilesController : Controller
    {
	  
	    private readonly IFilesService _filesService;
	    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();


	    public FilesController(IFilesService filesService)
	    {
		    _filesService = filesService;
	    }


		// GET: Files
		[Authorize]
		public ActionResult Index()
		{
			return RedirectToAction("List");
		}
		[Authorize]
		public ActionResult List()
		{
			try
			{
				var model = _filesService.GetList();
				return View(model);
			}
			catch (Exception ex)
			{
				_logger.Debug(ex);
			}

			return null;
		}

		[Authorize]
		[HttpGet]
		public ActionResult UploadFile()
		{
			
			return View();
		}

		[Authorize]
		[HttpPost]
		public ActionResult UploadFile(UploadFileViewModel viewModel)
		{
			_filesService.UploadFile(viewModel);
			return RedirectToAction("List");
		}
		[Authorize]
		public ActionResult DeleteFile(int id)
		{
			_filesService.DeleteFile(id);
			return RedirectToAction("List");
		}
	}
}