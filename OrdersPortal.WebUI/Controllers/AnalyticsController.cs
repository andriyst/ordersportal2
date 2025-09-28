using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NLog;
using OrdersPortal.Application.Context;
using OrdersPortal.Application.Models;
using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Application.Services;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Models;
using OrdersPortal.Domain.Models.Analytics;
using OrdersPortal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrdersPortal.WebUI.Controllers
{
    public class AnalyticsController : Controller
    {

		private readonly IAccountRepository _accountRepository;
		private readonly IAccountService _accountService;
		private readonly IAnalyticsService _analyticsService;
		private readonly ApplicationContext _applicationContext;
		private Logger _logger;
		public UserManager<OrderPortalUser> UserManager { get; private set; }

		public AnalyticsController(IAccountRepository accountRepository, IAccountService accountService, IAnalyticsService analyticsService,
			ApplicationContext applicationContext)
		{
			_accountRepository = accountRepository;
			_accountService = accountService;
			_analyticsService = analyticsService;
			_applicationContext = applicationContext;
			_logger = LogManager.GetCurrentClassLogger();
			var dbContextProvider = DependencyResolver.Current.GetService<IDbContextProvider>();
			UserManager = new UserManager<OrderPortalUser>(new UserStore<OrderPortalUser>(dbContextProvider.DbContext));

		}
		// GET: Finance
		[Authorize]
		public ActionResult Index()
		{

			return RedirectToAction("OrderAnalytics");
		}
		[Authorize]
		public ActionResult OrderAnalytics()
		{

			var viewModel = _analyticsService.PrepareAnalyticsOrdersListViewModel(null);

			return View(viewModel);
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult OrderAnalytics(OrderAnalyticsListViewModel viewModel)
		{
			try
			{

				viewModel = _analyticsService.PrepareAnalyticsOrdersListViewModel(viewModel);

				viewModel.AnalyticsCategoryOrderList = _analyticsService.GetOrderAnalyticsList(viewModel.ContrAgentCode, viewModel.StartDate, viewModel.EndDate);

				viewModel.ModelCharts = _analyticsService.GetOrderAnalyticsChartsData(viewModel.ContrAgentCode, viewModel.StartDate, viewModel.EndDate);

				viewModel.AnalyticsProfileSystemChartList = _analyticsService.GetProfileSystemAnalytics(viewModel.ContrAgentCode, viewModel.StartDate, viewModel.EndDate);

				if (viewModel.AnalyticsCategoryOrderList != null && viewModel.AnalyticsCategoryOrderList.Count > 0)
				{
					viewModel.AvarageBill = Math.Round(viewModel.AnalyticsCategoryOrderList.Sum(x => x.AnalyticsOrderList.Sum(y => y.TotalValue)) / viewModel.AnalyticsCategoryOrderList.Sum(x => x.AnalyticsOrderList.Count), 2);
				}
				return View(viewModel);
			}
			 catch (Exception ex)
			{
				_logger.Error(ex);
				return null;
			}
		}


	}
}