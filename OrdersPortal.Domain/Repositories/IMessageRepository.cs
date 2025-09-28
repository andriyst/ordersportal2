using System.Collections.Generic;

using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Domain.Repositories
{
	public interface IMessageRepository : IRepository<OrdersMessage>
	{
		OrdersMessage GetLastOrderMessage(int orderId);
		List<OrdersMessage> GetOrderMessages(int orderId);
	}
}
