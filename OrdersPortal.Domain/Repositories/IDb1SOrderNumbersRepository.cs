using System.Collections.Generic;
using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Domain.Repositories
{
	public interface IDb1SOrderNumbersRepository : IRepository<Db1SOrderNumbers>
	{
		Db1SOrderNumbers GetDb1SOrderNumberByNumber(string orderNumber);
		List<Db1SOrderNumbers> GetListByOrderId(int orderId);
		Db1SOrderNumbers GetByIdIncludes(int db1SOrderNumberId);
		List<string> GetNumbersListByOrderId(int orderId);
	}
}
