using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NLog;

using OrdersPortal.Application.Models;
using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Application.Services;
using OrdersPortal.Domain.Dto.Customer1cOrder;
using OrdersPortal.Domain.Models;

namespace OrdersPortal.WebUI.Controllers
{
	public class FinanceController : Controller
	{

		private readonly IFinanceService _financeService;
		private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
		private const int _topWdsRatingNumber = 10;
		private const int _daysInWdsRating = 1;


		public FinanceController(IFinanceService financeService)
		{

			_financeService = financeService;
		}


		// GET: Finance
		[Authorize]
		public ActionResult Index()
		{

			return RedirectToAction("List");
		}

		[Authorize]
		public ActionResult List()
		{
			var viewModel = _financeService.PrepareFinanceOrdersListViewModel();
			return View(viewModel);
		}


		[Authorize]
		public JsonResult TableDataGetFinanceOrders(FinanceTableDataModel financeTableDataModel)
		{
			try
			{
				IList<FinanceOrder> tableData = _financeService.GetFinanceOrderList(financeTableDataModel);

				JsonFinanceDataTableModel jsonTableData = new JsonFinanceDataTableModel
				{
					rows = tableData.ToList().GetRange(financeTableDataModel.Offset, Math.Min(financeTableDataModel.Limit, tableData.Count - financeTableDataModel.Offset)),
					total = tableData.Count,
					SumTotalValue = tableData.Sum(x => x.TotalValue),
					SumAdvanceValue = tableData.Sum(x => x.AdvanceValue),
					SumBalanceValue = tableData.Sum(x => x.BalanceValue),
					SumQuantityConstructions = tableData.Sum(x => x.QuantityConstructions)
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
		public ActionResult CashFlow()
		{

			var viewModel = _financeService.PrepareFinanceCashFlowViewModel(null);

			return View(viewModel);
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CashFlow(FinanceCashFlowViewModel viewModel)
		{
			try
			{

				viewModel = _financeService.PrepareFinanceCashFlowViewModel(viewModel);
				viewModel.FinanceDayCashFlowList = _financeService.GetFinanceDayCashFlowList(viewModel.ContrAgentCode, viewModel.StartDate, viewModel.EndDate);

				return View(viewModel);
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
				return null;
			}
		}

		[Authorize]
		public ActionResult CashFlowExportCsv(FinanceCashFlowViewModel viewModel)
		{
			try
			{
				viewModel = _financeService.PrepareFinanceCashFlowViewModel(viewModel);

				List<FinanceDayCashFlow> financeDayCashFlowList = _financeService.GetFinanceDayCashFlowList(viewModel.ContrAgentCode, viewModel.StartDate, viewModel.EndDate);
				FileContentResult file = _financeService.CashFlowGenerateFile(financeDayCashFlowList);

				return file;
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
				return null;
			}
		}

		[Authorize]
		public ActionResult Reconciliation()
		{
			var viewModel = _financeService.PrepareFinanceReconciliationViewModel(null);

			return View(viewModel);
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Reconciliation(FinanceReconciliationViewModel viewModel)
		{
			try
			{
				viewModel = _financeService.PrepareFinanceReconciliationViewModel(viewModel);

				viewModel.FinanceReconciliationList = _financeService.GetFinanceReconciliationList(viewModel.ContrAgentCode, viewModel.StartDate, viewModel.EndDate);
				return View(viewModel);
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
				return null;
			}
		}
		[Authorize]
		public ActionResult ReconciliationExportCsv(FinanceReconciliationViewModel viewModel)
		{
			try
			{
				viewModel = _financeService.PrepareFinanceReconciliationViewModel(viewModel);

				var financeReconciliationList = _financeService.GetFinanceReconciliationList(viewModel.ContrAgentCode, viewModel.StartDate, viewModel.EndDate);
				FileContentResult file = _financeService.ReconciliationGenerateFile(financeReconciliationList);

				return file;
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
				return null;
			}
		}

		[Authorize]
		public ActionResult OrdersPayments()
		{

			OrdersPaymentsViewModel viewModel = _financeService.PrepareOrdersPaymentsViewModel();

			return View(viewModel);
		}

		[Authorize]
		[HttpGet]
		public ActionResult GetWdsActionCodes()
		{
			WdsActionCodeViewModel model = _financeService.PrepareWdsActionViewModel();

			return View(model);
		}

		[Authorize]
		[HttpPost]
		public ActionResult GetWdsActionCodes(WdsActionCodeViewModel model)
		{
			model = _financeService.GetWdsCodes(model);

			return View(model);
		}
		[Authorize]
		public ActionResult GetOrdersPayments(OrdersPaymentsViewModel viewModel)
		{

			//var viewModel = new OrdersPaymentsViewModel();
			try
			{
				viewModel = _financeService.GetGetOrderPayments(viewModel);
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
			}
			return PartialView("~/Views/Finance/GetOrdersPayments.cshtml", viewModel);

		}
		[Authorize]
		public ActionResult GetWdsActionRating()
		{
			List<WdsActionRatingListViewModel> viewModel = _financeService.GetWdsActionRating(_topWdsRatingNumber, _daysInWdsRating);

			return View(viewModel);
		}

		[Authorize]
		[HttpPost]
		public ActionResult SetOrdersPayments(OrdersPaymentsViewModel model)
		{
			try
			{

				if ((ModelState.IsValid && model.AvailableAdvance >= model.Payments.Sum(x => x.Payment)))
				{
					if (!_financeService.ProceedOrdersPayments(model))
					{
						_logger.Error($"Payment customer: {model.ContrAgentCode} was delayed as exist payment is progress");
						ViewBag.Error = "Delay";
						return View();
					}
				}
				else
				{
					_logger.Error($"Payment customer: {model.ContrAgentCode} has errors");
					ViewBag.Error = "Error";
					return View();
				}

			}
			catch (Exception ex)
			{
				_logger.Error(ex);
				ViewBag.Error = "Error";
				return View();
			}
			return View();
		}

		[Authorize]
		public ActionResult GetCustomerBillList()
		{

			var viewModel = _financeService.GetCustomerBillList(null);
			return View(viewModel);
		}

		[Authorize]
		[HttpPost]
		public ActionResult GetCustomerBillList(CustomerBillListViewModel viewModel)
		{
			viewModel = _financeService.GetCustomerBillList(viewModel);
			return View(viewModel);
		}

		[Authorize]
		public ActionResult CreateCustomerBill()
		{

			var viewModel = _financeService.GetCustomerBillValues();
			return View(viewModel);
		}

		[Authorize]
		public ActionResult DownloadCustomerBillByGuid(string guid)
		{
			var file = _financeService.GetCustomerBillByGuid(guid);

			byte[] data = Convert.FromBase64String(file);



			return File(data, "application/pdf");



			//string url = "<object style='width:80px; height:60px;' > <a target=\"blank\" alt=\"Відкрити в новому вікні\" href=\"data:application/pdf;base64,"
			//             + viewModel + "\">order.pdf</a> </object>";
			//return Json(url, JsonRequestBehavior.AllowGet);

		}

		[Authorize]
		[HttpPost]
		public ActionResult CreateCustomerBill(Create1COrderViewModel model)
		{

			_financeService.CreateCustomerBill(model);
			return RedirectToAction("GetCustomerBillList");
		}

	}
}