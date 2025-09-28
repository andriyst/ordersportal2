using OrdersPortal.Domain.Models;
using OrdersPortal.Domain.Models.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class OrderAnalyticsListViewModel : AnalyticsBaseViewModel
	{
		public List<AnalyticsCategoryOrder> AnalyticsCategoryOrderList { get; set; }
		public decimal AvarageBill { get; set; }
		public List<AnalyticsProfileSystem> AnalyticsProfileSystemChartList { get; set; }

		public List<AnalyticChartModel> ModelCharts { get; set; }
	}

	public class AnalyticChartModel
	{
		public string Name { get; set; }
		public string Title { get; set; }
		public List<ChartData> ChartData { get; set; }
	}

	public class ChartData
	{
		public string Name { get; set; }
		public double Value { get; set; }
	}
}
