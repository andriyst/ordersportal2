using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.Infrastructure.Repositories
{
	public class Db1SOrderPartsNumbersRepository : Repository<Db1SOrderPartsNumbers>, IDb1SOrderPartsNumbersRepository
	{
		public Db1SOrderPartsNumbersRepository(UnitOfWork unitOfWork) : base(unitOfWork)
		{
		}

		public Db1SOrderPartsNumbers GetDb1SOrderPartsNumberByNumber(string orderPartsNumber)
		{
			return DbSet.FirstOrDefault(x => x.Db1SOrderPartsNumber == orderPartsNumber);
		}

		public string GetNumberByOrderPartsId(int orderPartId)
		{
			return DbSet.FirstOrDefault(x => x.OrderPartsId == orderPartId)?.Db1SOrderPartsNumber;
		}

		public Db1SOrderPartsNumbers GetByOrderPartsId(int orderPartId)
		{
			return DbSet.FirstOrDefault(x => x.OrderPartsId == orderPartId);
		}

		public Db1SOrderPartsNumbers GetByIdIncludes(int db1SOrderPartsNumberId)
		{
			return DbSet.Include(x => x.OrderParts).FirstOrDefault(x => x.Db1SOrderPartsNumberId == db1SOrderPartsNumberId);
		}
	}
}
