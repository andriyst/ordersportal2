using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using NLog;

using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Application.Services;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;
using OrdersPortal.Infrastructure.EntityFramework;

namespace OrdersPortal.WebUI.Controllers
{
	public class StatsController : Controller
	{

	
		private readonly IStatService _statService;
		private Logger _logger;
		//public UserManager<OrderPortalUser> UserManager { get; private set; }

		public StatsController(IStatService statService)
		{

			_statService = statService;
			_logger = LogManager.GetCurrentClassLogger();
			//var dbContextProvider = DependencyResolver.Current.GetService<IDbContextProvider>();
			//UserManager = new UserManager<OrderPortalUser>(new UserStore<OrderPortalUser>(dbContextProvider.DbContext));

		}
		

		public ActionResult HelpServiceStat(string startDateHs, string endDateHs)
		{


			HelpServiceStatsViewModel model = _statService.GetHelpServiceStats(startDateHs, endDateHs);
			return View(model);
		}

	}
}