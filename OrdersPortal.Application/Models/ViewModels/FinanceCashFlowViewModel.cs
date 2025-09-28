using System.Collections.Generic;
using OrdersPortal.Domain.Models;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class FinanceCashFlowViewModel : FinanceBaseViewModel
	{
		public List<FinanceDayCashFlow> FinanceDayCashFlowList { get; set; }

	}
}
