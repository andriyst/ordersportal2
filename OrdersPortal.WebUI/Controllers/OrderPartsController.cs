using NLog;
using OrdersPortal.Application.Context;
using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Application.Services;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Helpers;
using OrdersPortal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace OrdersPortal.WebUI.Controllers
{

	[Authorize]
	public class OrderPartsController : Controller
	{
		private readonly IOrderPartsRepository _orderPartsRepository;

		private readonly IAccountService _accountService;
		private readonly IOrderPartsService _orderPartsService;
		private readonly IOrderService _orderService;

		private readonly ApplicationContext _applicationContext;

		private readonly Logger _logger;

		private int[] _showStatusIds = { };
		private readonly int[] _showStatusUploads = { 1, 2 };
		private readonly int[] _showStatusNotConfirm = { 23 };
		private readonly int[] _showStatusNotPayed = { 22 };
		private readonly int[] _showStatusInWork = { 14, 16 };
		private readonly int[] _showStatusDecline = { 7, 17, 20, 21 };
		private readonly int[] _showStatusAll = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 25 };

		private readonly int[] _showStatusInStock = { 9 };
		private readonly int[] _showStatusSolved = { 8 };
		private readonly int[] _showStatusInOrder = { 3 };
		private readonly int[] _showStatusInDelivery = { 25 };
		private readonly int[] _showStatusInProduction = { 6, 16 };

		public OrderPartsController(IOrderPartsRepository orderPartsRepository, ApplicationContext applicationContext,
			 IAccountService accountService, IOrderPartsService orderPartsService, IOrderService orderService)
		{
			_orderPartsRepository = orderPartsRepository;
			_applicationContext = applicationContext;
			_accountService = accountService;
			_orderPartsService = orderPartsService;
			_orderService = orderService;
			_logger = LogManager.GetCurrentClassLogger();
		}

		// GET: OrderParts

		[Authorize]
		public ActionResult Index()
		{
			return RedirectToAction("List");
		}

		[Authorize]
		public ActionResult List()
		{

			return View();
		}


		[Authorize]
		public ActionResult Add()
		{
			OrderPortalUser currentUser = _accountService.GetById(_applicationContext.AccountId);

			//AddOrderPartsViewModel order = new AddOrderPartsViewModel
			//{
			//	CustomerName = currentUser.UserName,
			//	OrderPartsNumber = _orderPartsRepository.GetOrderPartsNumber(currentUser.Id)
			//};
			AddOrderPartsViewModel orderParts = _orderPartsService.PrepareAddViewModel();

			return View(orderParts);
		}

		[Authorize]
		[HttpPost]
		public ActionResult Add(AddOrderPartsViewModel viewModel)
		{

			if (ModelState.IsValid)
			{
				_orderPartsService.AddOrderParts(viewModel);
			}
			else
			{
				OrderPortalUser currentUser = _accountService.GetById(_applicationContext.AccountId);
				viewModel.CustomerName = currentUser.UserName;
				return View(viewModel);
			}

			return RedirectToAction("List");
		}


		[Authorize]
		public JsonResult TableDataGetOrderParts(OrderPartsTableDataModel tableDataModel, int? status)
		{
			try
			{

				OrderPortalUser currentUser = _accountService.GetById(_applicationContext.AccountId);


				IQueryable<OrderParts> entitySet;


				switch (status)
				{
					case 1:
						_showStatusIds = _showStatusUploads;
						break;
					case 2:
						_showStatusIds = _showStatusInOrder;
						break;
					case 3:
						_showStatusIds = _showStatusNotPayed;
						break;
					case 4:
						_showStatusIds = _showStatusInProduction;
						break;
					case 5:
						_showStatusIds = _showStatusInStock;
						break;
					case 6:
						_showStatusIds = _showStatusInDelivery;
						break;
					default:
						_showStatusIds = _showStatusAll;
						break;
				}

				tableDataModel.Statuses = _showStatusIds;

				if (User.IsInRole("customer"))
				{
					tableDataModel.CustomerId = currentUser.Id;


				}
				else if (User.IsInRole("manager"))
				{

					tableDataModel.ManagerId = currentUser.Id;

				}
				else if (User.IsInRole("regionmanager"))
				{

					tableDataModel.RegionManagerId = currentUser.Id;

				}

				entitySet = _orderPartsRepository.GetSortAndSearchList(tableDataModel);

				var count = entitySet.Count();
				var entity = entitySet.Skip(tableDataModel.Offset).Take(tableDataModel.Limit)
									  .Include(x => x.Status)
									  .Include(c => c.Customer)
									  .Include(m => m.Manager)
									  .Include(m => m.OrderPartsItem)
									  .Include(m => m.OrderPartsReason)
									  .ToList();

				IList<OrderPartsListViewModel> tableData = OrderPartsListViewModel.ConvertFromEntities(entity).ToList();

				foreach (var row in tableData)
				{
					row.Db1SOrderPartsNumbers = _orderPartsService.Get1COrderPartsNumberByOrderId(row.OrderPartsId);

				}
				var jsonTableData = new JsonDataTableModel<OrderPartsListViewModel>
				{
					rows = tableData,
					total = count
				};

				return Json(jsonTableData, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
				return null;
			}
		}

		[Authorize]
		public ActionResult ShowOrderPartsDetail(int orderPartsId)
		{
			ViewBag.OrderPartsId = orderPartsId;
			var db1SOrderPartsDetail = _orderPartsService.Get1COrderPartsByOrderPartsId(orderPartsId);

			return PartialView("~/Views/OrderParts/Partials/PartialsShowOrderPartsDetail.cshtml", db1SOrderPartsDetail);
		}

		[HttpGet]
		[Authorize]
		public FileResult DownloadOrderPartsFile(string db1cOrderPartsNumber)
		{
			var document = _orderPartsService.Get1COrderPartsNumberFileByOrderPartsId(db1cOrderPartsNumber);

			return File(document, "application/pdf", "add-order.pdf");
		}

		[Authorize]
		public ActionResult ConfirmOrder(int db1COrderPartId)
		{
			try
			{
				_orderPartsService.Confirm1SOrderParts(db1COrderPartId);
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
			}

			return RedirectToAction("List");
		}

	}
}