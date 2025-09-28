using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Infrastructure.Repositories
{
	public class OrderPartsReasonRepository : Repository<OrderPartsReason>, IOrderPartsReasonRepository
	{
		public OrderPartsReasonRepository(UnitOfWork unitOfWork) : base(unitOfWork)
		{

		}
		public IList<OrderPartsReason> GetList()
		{
			return DbSet.ToList();
		}
	}
}
