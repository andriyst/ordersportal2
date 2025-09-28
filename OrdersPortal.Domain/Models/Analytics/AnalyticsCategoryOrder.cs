using OrdersPortal.Domain.Models.Analytics;
using System;
using System.Collections.Generic;

namespace OrdersPortal.Domain.Models
{
	public class AnalyticsCategoryOrder
	{
		
		public List<AnalyticsOrder> AnalyticsOrderList { get; set; }		
		
		public string Category { get; set; }
	
	}
}
