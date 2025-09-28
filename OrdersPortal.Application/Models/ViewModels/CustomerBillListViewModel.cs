using System.Collections.Generic;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class CustomerBillListViewModel : FinanceBaseViewModel
	{
		public List<CustomerBillViewModel> CustomerBillList { get; set; }
	
	}
}
