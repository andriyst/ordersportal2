using System.Collections.Generic;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class OrderMessageListViewModel
	{
		public List<OrderMessageViewModel> OrdersMessageList { get; set; }
		public int OrderId { get; set; }
		public string Message { get; set; }
	}
}
