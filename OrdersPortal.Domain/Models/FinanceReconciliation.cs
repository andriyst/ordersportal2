
namespace OrdersPortal.Domain.Models
{
	public class FinanceReconciliation
	{
		public string Document { get; set; }
		public string OrderNumber { get; set; }
		public decimal InitialBalance { get; set; }
		public decimal IncomeValue { get; set; }
		public decimal OutcomeValue { get; set; }
		public decimal FinalBalance { get; set; }

	}
}
