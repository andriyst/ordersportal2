using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.Infrastructure.Repositories
{
	public class Db1SOrderNumbersRepository : Repository<Db1SOrderNumbers>, IDb1SOrderNumbersRepository
	{
		public Db1SOrderNumbersRepository(UnitOfWork unitOfWork) : base(unitOfWork)
		{
		}

		public Db1SOrderNumbers GetDb1SOrderNumberByNumber(string orderNumber)
		{
			return DbSet.FirstOrDefault(x => x.Db1SOrderNumber == orderNumber);
		}

		public List<Db1SOrderNumbers> GetListByOrderId(int orderId)
		{
			return DbSet.Where(x => x.OrderId == orderId).ToList();
		}

		public Db1SOrderNumbers GetByIdIncludes(int db1SOrderNumberId)
		{
			return DbSet.Include(x => x.Order).FirstOrDefault(x => x.Db1SOrderNumberId == db1SOrderNumberId);
		}

		public List<string> GetNumbersListByOrderId(int orderId)
		{
			return DbSet.Where(x => x.OrderId == orderId).Select(r=>r.Db1SOrderNumber).ToList();
		}
	}
}
