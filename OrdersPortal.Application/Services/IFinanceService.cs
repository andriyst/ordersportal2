
using System;
using System.Collections.Generic;
using System.Web.Mvc;

using OrdersPortal.Application.Models;
using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Domain.Dto.Customer1cOrder;
using OrdersPortal.Domain.Models;

namespace OrdersPortal.Application.Services
{
	public interface IFinanceService
	{
		FinanceOrdersListViewModel PrepareFinanceOrdersListViewModel();
		IList<FinanceOrder> GetFinanceOrderList(FinanceTableDataModel financeTableDataModel);
		FinanceCashFlowViewModel PrepareFinanceCashFlowViewModel(FinanceCashFlowViewModel viewModel);
		List<FinanceDayCashFlow> GetFinanceDayCashFlowList(string contrAgentCode, DateTime startDate, DateTime endDate);
		FileContentResult CashFlowGenerateFile(List<FinanceDayCashFlow> financeDayCashFlowList);
		FinanceReconciliationViewModel PrepareFinanceReconciliationViewModel(FinanceReconciliationViewModel viewModel);
		FinanceReconciliationList GetFinanceReconciliationList(string contrAgentCode, DateTime startDate, DateTime endDate);
		FileContentResult ReconciliationGenerateFile(FinanceReconciliationList financeReconciliationList);
		OrdersPaymentsViewModel PrepareOrdersPaymentsViewModel(OrdersPaymentsViewModel viewModel = null);

		WdsActionCodeViewModel PrepareWdsActionViewModel();
		WdsActionCodeViewModel GetWdsCodes(WdsActionCodeViewModel model);
		List<WdsActionRatingListViewModel> GetWdsActionRating(int topNumber, int daysInWdsRating);

		Create1COrderViewModel GetCustomerBillValues();
		CustomerBillListViewModel GetCustomerBillList(CustomerBillListViewModel viewModel);
		bool CreateCustomerBill(Create1COrderViewModel model);
		string GetCustomerBillByGuid(string guid);
		OrdersPaymentsViewModel GetGetOrderPayments(OrdersPaymentsViewModel viewModel);
		bool ProceedOrdersPayments(OrdersPaymentsViewModel model);
	}
}
