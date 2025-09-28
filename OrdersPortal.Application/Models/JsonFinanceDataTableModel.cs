using OrdersPortal.Domain.Helpers;
using OrdersPortal.Domain.Models;

namespace OrdersPortal.Application.Models
{
	public class JsonFinanceDataTableModel : JsonDataTableModel<FinanceOrder>
	{
		public decimal SumTotalValue { get; set; }
		public decimal SumAdvanceValue { get; set; }
		public decimal SumBalanceValue { get; set; }
		public decimal SumQuantityConstructions { get; set; }
	}
}
