using System.Collections.Generic;
using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Domain.Repositories
{
	public interface IDb1SOrderPartsNumbersRepository : IRepository<Db1SOrderPartsNumbers>
	{
		Db1SOrderPartsNumbers GetDb1SOrderPartsNumberByNumber(string orderPartsNumber);
		string GetNumberByOrderPartsId(int orderPartId);
		Db1SOrderPartsNumbers GetByOrderPartsId(int orderPartId);
		Db1SOrderPartsNumbers GetByIdIncludes(int db1SOrderPartsNumberId);
	}
}
