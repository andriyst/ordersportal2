using System.Collections.Generic;
using OrdersPortal.Domain.Models;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class FinanceOrdersListViewModel : FinanceBaseViewModel
	{
		public List<FinanceOrder> FinanceOrderList { get; set; }

	}
}
