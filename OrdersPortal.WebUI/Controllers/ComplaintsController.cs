using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using NLog;
using OrdersPortal.Application.Context;
using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Application.Services;
using OrdersPortal.Domain.Dto;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Helpers;
using OrdersPortal.Domain.Repositories;


namespace OrdersPortal.WebUI.Controllers
{
	public class ComplaintsController : Controller
	{

		private readonly IOrderRepository _orderRepository;
		private readonly IComplaintsRepository _complaintsRepository;
		private readonly IOrderService _orderService;
		private readonly IComplaintsService _complaintsService;
		private readonly IMessageRepository _messageRepository;
		private readonly IAccountService _accountService;
		private readonly IMessageService _messageService;
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
		private readonly int[] _showStatusInProduction = { 6,16 };

		public ComplaintsController(IOrderRepository orderRepository, IOrderService orderService,
			IComplaintsRepository complaintsRepository, IComplaintsService complaintsService, IAccountService accountService,
			IMessageRepository messageRepository, IMessageService messageService, ApplicationContext applicationContext)
		{
			_orderRepository = orderRepository;
			_complaintsRepository = complaintsRepository;
			_orderService = orderService;
			_complaintsService = complaintsService;
			_messageRepository = messageRepository;

			_accountService = accountService;
			_messageService = messageService;
			_applicationContext = applicationContext;
			_logger = LogManager.GetCurrentClassLogger();
		}

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
		public ActionResult ShowComplaintsList(int? page)
		{
			return RedirectToAction("List");
		}

		[HttpGet]
		public ActionResult ShowComplaintsListByStatus(string status, int? page)
		{

			return RedirectToAction("List");
		}


		public ActionResult AddComplaint()
		{
			var model = _complaintsService.PrepareUploadComplaintViewModel();

			return View(model);
		}



		[HttpPost]
		public ActionResult AddComplaint(UploadComplaintViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				_complaintsService.UploadComplaint(viewModel);
			}
			else
			{
				viewModel = _complaintsService.PrepareUploadComplaintViewModel();
				viewModel.ComplaintIssue = 0;

				return View(viewModel);
			}
			return RedirectToAction("List");
		}



		public ActionResult EditComplaint(int complaintid)
		{

			var model = _complaintsService.EditComplaint(complaintid);

			if (TempData["ViewData"] != null)
			{
				ViewData = (ViewDataDictionary)TempData["ViewData"];
				TempData["ViewData"] = null;
			}

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditComplaint(EditComplaintViewModel complaint)
		{

			if (ModelState.IsValid)
			{
				_complaintsService.SaveComplaint(complaint);

			}
			else
			{
				TempData["ViewData"] = ViewData;
				return RedirectToAction("EditComplaint", new { complaintid = complaint.ComplaintId });
			}
			return RedirectToAction("List");


		}
		public ActionResult ShowComplaintDetail(int complaintid)
		{

			var model = _complaintsService.GetComplaintDetailsByComplaintId(complaintid);
			return PartialView("~/Views/Complaints/Partials/PartialsShowComplaintDetails.cshtml", model);
		}
		public ActionResult ShowPhoto(int photoid)
		{
			var photo = _complaintsService.GetPhotoById(photoid);

			return PartialView("~/Views/Complaints/Partials/PartialsShowPhoto.cshtml", photo);
		}

		public ActionResult CheckOrder(string ordernum)
		{
			if (_complaintsService.CheckContrAgentOrder(ordernum))
			{
				ViewBag.Checked = "checkorderyes";
				ViewBag.CheckedText = "Замовлення знайдено";
			}
			else
			{
				ViewBag.Checked = "checkorderno";
				ViewBag.CheckedText = "Замовлення не знайдено";
			}
			return PartialView("~/Views/Complaints/Partials/CheckOrderBox.cshtml");
		}

		public ActionResult GetOrderSeries(string orderNumber)
		{
			List<ComplaintOrderSerieDto> orderSerie = _complaintsService.GetComplaintOrderSeriesByOrderNumber(orderNumber);

			return PartialView("~/Views/Complaints/Partials/OrderSeriesBox.cshtml", orderSerie);

		}
		public JsonResult GetSolutions(int id)
		{

			var result = _complaintsService.GetSolutionsByIssue(id);
			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult ApproveSolution(int complaintId)
		{
			_complaintsService.ApproveComplaint(complaintId);

			return Redirect("ShowComplaintsList");
		}

		public ActionResult FillIssueSolutionList()
		{
			string result = "Список Проблем та Рішень завантажено успішно!";
			if (!_complaintsService.LoadIssueAndSolutionList())
			{
				result = "Сталася помилка!";
			}
			ViewBag.Text = result;
			return View();
		}

		[Authorize]
		public JsonResult TableDataGetComplaints(ComplaintTableDataModel tableDataModel, int? status)
		{
			try
			{

				OrderPortalUser currentUser = _accountService.GetById(_applicationContext.AccountId);
				var currentUserOrganizationList = currentUser.OrderPortalUserOrganizations.Select(o => o.Organization).ToList();


				IQueryable<Complaint> entitySet;

		//				private readonly int[] _showStatusInStock = { 9 };
		//private readonly int[] _showStatusSolved = { 8 };
		//private readonly int[] _showStatusInOrder = { 3 };
		//private readonly int[] _showStatusInProduction = { 6, 16 };
				switch (status)
				{
					case 1:
						_showStatusIds = _showStatusUploads;
						break;
					case 2:
						_showStatusIds = _showStatusInOrder;
						break;
					case 3:
						_showStatusIds = _showStatusInStock;
						break;
					case 4:
						_showStatusIds = _showStatusInProduction;
						break;
					case 5:
						_showStatusIds = _showStatusSolved;
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

				entitySet = _complaintsRepository.GetSortAndSearchList(tableDataModel);

				var count = entitySet.Count();
				var entity = entitySet.Skip(tableDataModel.Offset).Take(tableDataModel.Limit)
									  .Include(x => x.Status)
									  .Include(c => c.Customer)
									  .Include(b1 => b1.Customer.OrderPortalUserOrganizations)
									  .Include(b2 => b2.Customer.OrderPortalUserOrganizations.Select(t => t.Organization))
									  .Include(m => m.Manager)
									  .Include(m => m.ComplaintDecisions)
									  .Include(m => m.ComplaintDecisions.Select(v => v.ComplaintSolution))
									  .Include(n => n.ComplaintDecisions.Select(w => w.ComplaintIssue))
									  .ToList();

				IList<ComplaintListViewModel> tableData = ComplaintListViewModel.ConvertFromEntities(entity).ToList();


				var jsonTableData = new JsonDataTableModel<ComplaintListViewModel>
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
	}
}

