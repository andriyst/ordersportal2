using System.Collections.Generic;
using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Domain.Repositories
{
	
	public interface IOrderPartsRepository : IRepository<OrderParts>
	{
		List<OrderParts> GetList();
		//string GetFilePath(int id);
		string GetOrderPartsNumber(string customerId);
		//OrderParts GetOrderPartsByDb1cOrderPartsNumber(string db1COrderPartsNumber);

	}
}
