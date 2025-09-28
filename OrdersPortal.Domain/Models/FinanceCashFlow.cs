using System;

namespace OrdersPortal.Domain.Models
{
	public class FinanceCashFlow
	{
		public DateTime CreateDate { get; set; }
		public string OrderNumber { get; set; }
		public string Document { get; set; }
		public decimal BeginPeriod { get; set; }
		public decimal IncomeValue { get; set; }
		public decimal OutcomeValue { get; set; }
		public string EndPeriod { get; set; }
	}
}
