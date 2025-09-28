using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using NLog;
using OrdersPortal.Application.Context;
using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Application.Services;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Helpers;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.WebUI.Controllers
{
	[Authorize]
	public class OrdersController : Controller
	{

		private readonly IOrderRepository _orderRepository;
		private readonly IOrderService _orderService;
		private readonly IMessageRepository _messageRepository;
		private readonly IAccountService _accountService;
		private readonly IMessageService _messageService;
		private readonly IOrganizationRepository _organizationRepository;
		private readonly ApplicationContext _applicationContext;

		private readonly Logger _logger;
		private int[] _showStatusIds = { };
		private readonly int[] _showStatusUploads = { 1 };
		private readonly int[] _showStatusInProgress = { 2, 3 };
		private readonly int[] _showStatusNotConfirm = { 23 };
		private readonly int[] _showStatusNotPayed = { 22 };
		private readonly int[] _showStatusInWork = { 14 };
		private readonly int[] _showStatusDecline = { 7, 17, 20, 21 };
		private readonly int[] _showStatusAll = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };



		public OrdersController(IOrderRepository orderRepository, IOrderService orderService, IAccountService accountService, IOrganizationRepository organizationRepository,
			IMessageRepository messageRepository, IMessageService messageService, ApplicationContext applicationContext)
		{
			_orderRepository = orderRepository;
			_orderService = orderService;
			_messageRepository = messageRepository;

			_organizationRepository = organizationRepository;

			_accountService = accountService;
			_messageService = messageService;
			_applicationContext = applicationContext;
			_logger = LogManager.GetCurrentClassLogger();
		}
		public OrdersController()
		{

			_logger = LogManager.GetCurrentClassLogger();


		}

		public ActionResult Index()
		{
			return RedirectToAction("List");
		}

		[Authorize]
		public ActionResult List()
		{

			return View();
		}

		public ActionResult AddOrder()
		{
			OrderPortalUser currentUser = _accountService.GetById(_applicationContext.AccountId);

			UploadOrderViewModel order = new UploadOrderViewModel
			{
				CustomerName = currentUser.UserName,
				OrderNumber = _orderRepository.GetOrderNumber(currentUser.Id)
			};

			return View(order);
		}


		[HttpPost]
		public ActionResult AddOrder(UploadOrderViewModel order)
		{

			if (ModelState.IsValid && !_orderService.CheckContrAgentOrder(order.OrderNumber))
			{
				_orderService.UploadOrder(order);
			}
			else
			{
				OrderPortalUser currentUser = _accountService.GetById(_applicationContext.AccountId);
				order.CustomerName = currentUser.UserName;
				return View(order);
			}
			return RedirectToAction("List");
		}

		[Authorize(Roles = "admin")]
		public ActionResult DelOrder(int orderId)
		{

			_orderService.RemoveOrder(orderId);

			return RedirectToAction("List");
		}

		[HttpPost]
		public void ChangeOrderStatus(int orderId, string statusName)
		{

			if (User.IsInRole("manager") || User.IsInRole("operator"))
			{
				_orderService.ChangeOrderStatusById(orderId, statusName);
			}
		}

		[HttpPost]
		public ActionResult ShowAllMessages(int orderId)
		{
			OrderMessageListViewModel model = new OrderMessageListViewModel();
			try
			{
				var messages = _messageService.GetOrderMessages(orderId);

				//model.Userid = _userRoleRepository.GetByCustomerName(User.Identity.Name).Id;
				model.OrdersMessageList = messages;
				model.OrderId = orderId;
			}
			catch (Exception ex)
			{
				ex = ex;
			}

			return PartialView("~/Views/Orders/Partials/PartialsShowAllMessages.cshtml", model);
		}

		[HttpPost]
		public ActionResult AddCommentMessage(OrderMessageViewModel model)
		{
			model.MessageWriterId = _applicationContext.AccountId;
			_messageService.AddOrderMessage(model);
			return RedirectToAction("List");
		}

		public ActionResult ConfirmOrder(int db1COrderId)
		{
			try
			{
				_orderService.Confirm1SOrder(db1COrderId);
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
			}

			return RedirectToAction("List");
		}


		//public ActionResult ChangeManager(int orderid)
		//{
		//	IEnumerable<OrderPortalUser> managers = _userRoleRepository.GetAllUsersByRoleName("manager");
		//	ViewBag.Managers = managers;
		//	ViewBag.OrderId = orderid;

		//	return PartialView("~/Views/Orders/Partials/PartialsChangeManager.cshtml");
		//}
		//[HttpPost]
		//public ActionResult ChangeManager(Order model)
		//{
		//	int orderId = model.OrderId;
		//	string managerId = model.ManagerId;

		//	_orderRepository.ChangeManager(orderId, managerId);

		//	return RedirectToAction("List");
		//}
		public ActionResult ShowOrderDetail(int orderId)
		{
			ViewBag.OrderId = orderId;
			var db1SOrdersDetail = _orderService.Get1COrdersByOrderId(orderId);

			return PartialView("~/Views/Orders/Partials/PartialsShowOrderDetail.cshtml", db1SOrdersDetail);
		}

		[Authorize]
		public JsonResult TableDataGetOrders(OrderTableDataModel tableDataModel, int? status)
		{
			try
			{
				if (tableDataModel.Sort == null)
				{
					tableDataModel.Sort = "LastMessageTime";
					tableDataModel.Order = "desc";
				}

				OrderPortalUser currentUser = _accountService.GetById(_applicationContext.AccountId);
				int regionId = currentUser.RegionId ?? 0;
				IQueryable<Order> entitySet;

				var currentUserOrganizationList = currentUser.OrderPortalUserOrganizations.Select(o => o.Organization).ToList();

				switch (status)
				{
					case 1:
						_showStatusIds = _showStatusUploads;
						break;
					case 2:
						_showStatusIds = _showStatusInProgress;
						break;
					case 3:
						_showStatusIds = _showStatusNotConfirm;
						break;
					case 4:
						_showStatusIds = _showStatusNotPayed;
						break;
					case 5:
						_showStatusIds = _showStatusInWork;
						break;
					case 6:
						_showStatusIds = _showStatusDecline;
						break;
					default:
						_showStatusIds = _showStatusAll;
						break;
				}

				tableDataModel.Statuses = _showStatusIds;

				tableDataModel.Organizations = currentUserOrganizationList.Select(x => x.OrganizationId).ToArray();

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
				entitySet = _orderRepository.GetSortAndSearchList(tableDataModel);

				var count = entitySet.Count();
				var entity = entitySet.Skip(tableDataModel.Offset).Take(tableDataModel.Limit)
									  //.Include(x=>x.Db1SOrderNumbers)									  									  
									  .Include(x => x.Status)
									  .Include(c => c.Customer)
									  .Include(b1 => b1.Customer.OrderPortalUserOrganizations)									  
									  .Include(b2 => b2.Customer.OrderPortalUserOrganizations.Select(t => t.Organization))
									  //.Include(x => x.Customer.Customer.OrderPortalUser.OrderPortalUserOrganizations.Select(o => o.Organization))
									  .Include(m => m.Manager);

				IList<OrderListViewModel> tableData = OrderListViewModel.ConvertFromEntities(entity).ToList();
				foreach (var row in tableData)
				{
					row.Db1SOrderNumbers = _orderService.Get1COrderNumberByOrderId(row.OrderId);
					OrdersMessage lastMessage = _messageRepository.GetLastOrderMessage(row.OrderId);
					if (lastMessage != null)
					{
						row.LastMessage = !String.IsNullOrEmpty(lastMessage.Message) ? lastMessage.Message : "";
						row.LastMessageTime = lastMessage.MessageTime.ToString("dd.MM.yyyy HH:mm");
						row.LastMessageWriter = lastMessage.MessageWriter.FullName;
					}
				}

				var jsonTableData = new JsonDataTableModel<OrderListViewModel>
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
		public JsonResult GetOrderFileLink(int id)
		{

			try
			{
				string url = _orderRepository.GetFilePath(id);
				return Json(url, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
				return null;
			}

		}
		[HttpGet]
		[Authorize]
		public FileResult DownloadOrderFile(string db1cOrderNumber)
		{
			var document = _orderService.Get1COrderNumberFileByOrderId(db1cOrderNumber);

			return File(document, "application/pdf", "order.pdf");
		}

	}
}