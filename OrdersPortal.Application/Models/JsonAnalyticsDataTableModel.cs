using OrdersPortal.Domain.Helpers;
using OrdersPortal.Domain.Models;
using OrdersPortal.Domain.Models.Analytics;

namespace OrdersPortal.Application.Models
{
	public class JsonAnalyticsDataTableModel : JsonDataTableModel<AnalyticsOrder>
	{
		public decimal SumTotalValue { get; set; }
		public decimal SumAdvanceValue { get; set; }
		public decimal SumBalanceValue { get; set; }
		public decimal SumQuantityConstructions { get; set; }
	}
}
