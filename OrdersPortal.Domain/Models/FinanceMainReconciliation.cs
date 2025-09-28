using System.Collections.Generic;

namespace OrdersPortal.Domain.Models
{
	public class FinanceMainReconciliation
	{
		public string Document { get; set; }
		public List<FinanceReconciliation> ReconciliationList { get; set; }
		public decimal TotalInitialBalance { get; set; }
		public decimal TotalIncomeValue { get; set; }
		public decimal TotalOutcomeValue { get; set; }
		public decimal TotalFinalBalance { get; set; }
	}
}
