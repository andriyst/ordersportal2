using System.Collections.Generic;
using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Domain.Repositories
{
	
	public interface IOrderRepository : IRepository<Order>
	{
		List<Order> GetList();
		string GetFilePath(int id);
		string GetOrderNumber(string customerId);
		Order GetOrderByDb1cOrderNumber(string db1COrderNumber);

	}
}
