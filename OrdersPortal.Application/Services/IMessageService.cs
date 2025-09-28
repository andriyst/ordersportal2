using System.Collections.Generic;
using OrdersPortal.Application.Models.ViewModels;

namespace OrdersPortal.Application.Services
{
	public interface IMessageService
	{
		void AddOrderMessage(OrderMessageViewModel orderMessage);
		List<OrderMessageViewModel> GetOrderMessages(int orderId);
	}
}
