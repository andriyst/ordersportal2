using System.Collections.Generic;
using OrdersPortal.Domain.Models;

namespace OrdersPortal.Application.Models
{
	public class FinanceReconciliationList
	{
		public List<FinanceMainReconciliation> FinanceMainReconciliationList { get; set; }
		public FinanceReconciliation FinanceAdvanceReconciliationList { get; set; }
		public decimal FullIncomeTotal { get; set; }
		public decimal FullOutcomeTotal { get; set; }
		public decimal FullBeginTotal { get; set; }
		public decimal FullEndTotal { get; set; }

	}
}
