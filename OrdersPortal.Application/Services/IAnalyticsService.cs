using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Domain.Models;
using OrdersPortal.Domain.Models.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Application.Services
{
	public interface IAnalyticsService
	{
		OrderAnalyticsListViewModel PrepareAnalyticsOrdersListViewModel(OrderAnalyticsListViewModel viewModel);
		List<AnalyticsCategoryOrder> GetOrderAnalyticsList(string contrAgentCode, DateTime startDate, DateTime endDate);
		List<AnalyticsProfileSystem> GetProfileSystemAnalytics(string contrAgentCode, DateTime startDate, DateTime endDate);

		List<AnalyticChartModel> GetOrderAnalyticsChartsData(string contrAgentCode, DateTime startDate, DateTime endDate);
	}
}
