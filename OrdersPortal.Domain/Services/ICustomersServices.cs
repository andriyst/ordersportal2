using System;
using System.Collections.Generic;
using OrdersPortal.Domain.Dto;
using OrdersPortal.Domain.Dto.Customer1cOrder;
using OrdersPortal.Domain.Models;
using OrdersPortal.Domain.Models.Analytics;

namespace OrdersPortal.Domain.Services
{
	public interface ICustomersServices
	{
		bool CheckOrderInPeriodService(int? kod, string orderNum, DateTime startDate);
		bool Set1CDeliveryStatus(string kod, string orderNum, string org1cCode = "000000001");
		bool Set1COrderPartsDeliveryStatus(string kod, int orderPartsId);
		List<Manager1c> GetManagerList(string org1cId);
		List<RegionManager1c> GetRegionManagerList(string org1cId);
		bool UpdateManagers(Update1cManagersDto model);
		List<Customer1c> GetManagerCustomerList(string managerGiud, Boolean isRegionManager = false);
		List<ComplaintOrderSerieDto> GetComplaintOrderSeriesByOrderNumber(string orderNumber, int orderId);
		List<ComplaintIssueDto> LoadIssueAndSolutionList();
		List<ComplaintIssueDto> GetComplaintSolutionsListByIssue(string issueGuid);
		List<CustomerCodesListDto> GetCustomerCodesList(string org1cCode = "000000001");
		IList<FinanceOrder> GetFinanceOrderList(FinanceTableDataModel financeTableDataModel, bool isPayment = false);
		IList<AnalyticsOrder> GetAnalyticOrderList(AnalyticsTableDataModel analyticTableDataModel, bool isPayment = false);
		List<AnalyticsCategoryOrder> GetOrderAnalyticsList(string contrAgentCode, DateTime startDate, DateTime endDate);
		List<AnalyticsProfileSystem> GetAnalyticsProfileSystem(string contrAgentCode, DateTime startDate, DateTime endDate);
		List<FinanceDayCashFlow> GetFinanceCashFlowList(string contrAgentCode, DateTime startDate, DateTime endDate);
		FinanceReconciliationListDto GetFinanceReconciliationList(string contrAgentCode, DateTime startDate, DateTime endDate);
		Customer1cOrderValues GetCustomerBill(string contrCode);
		bool CreateCustomerBill(Create1COrder model, string contrCode);
		List<CustomerBillListDto> GetCustomerBillList1c(string contrCode);
		string GetCustomerBillByGuid(string guid);
		List<WdsActionCodeListModel> GetWdsActionCodes(string contrAgentCode, DateTime startDate, DateTime endDate);
		List<WdsActionRatingListModel> GetWdsActionCustomerRating(DateTime startDate, DateTime endDate, int topNumber);
		List<CustomerCodesListDto> GetCustomerCodesListByOrganization(string org1cId);
		List<Organization1c> GetOrganizationList();
	}
}
