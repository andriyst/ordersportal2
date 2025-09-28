using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Infrastructure.Repositories
{
	public class OrderPartsItemRepository : Repository<OrderPartsItem>, IOrderPartsItemRepository
	{
		public OrderPartsItemRepository(UnitOfWork unitOfWork) : base(unitOfWork)
		{

		}
		public List<OrderPartsItem> GetList()
		{
			return DbSet.ToList();
		}
	}
}
