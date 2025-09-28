using System.Collections.Generic;
using OrdersPortal.Domain.Models;

namespace OrdersPortal.Domain.Dto
{
	public class FinanceCashFlowListDto
	{
		public List<FinanceCashFlow> FinanceCashFlowList { get; set; }
		public decimal TotalIncome { get; set; }





		public string Day { get; set; }
		public decimal Income { get; set; }
		public decimal TotalOutcome { get; set; }
		public decimal TotalStartIncome { get; set; }
		public decimal TotalStartIncomeEnd { get; set; }
		public string RestIncome { get; set; }
	}
}
