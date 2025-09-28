using System;
using System.Collections.Generic;

namespace OrdersPortal.Domain.Models
{
	public class FinanceDayCashFlow
	{
		public DateTime Day { get; set; }
	//	public decimal IncomeValue { get; set; }
		//public decimal OutcomeValue { get; set; }
		//public decimal BalanceValue { get; set; }
		public List<FinanceCashFlow> FinanceCashFlow { get; set; }
		//public decimal TotalIncomeValue { get; set; }
		//public decimal TotalOutcomeValue { get; set; }
		//public decimal TotalBalanceValue { get; set; }
		public decimal TotalIncome { get; set; }
		public decimal TotalOutcome { get; set; }
		public decimal TotalStartIncome { get; set; }
		public decimal TotalStartIncomeEnd { get; set; }
		public string RestIncome { get; set; }
	
	}
}
