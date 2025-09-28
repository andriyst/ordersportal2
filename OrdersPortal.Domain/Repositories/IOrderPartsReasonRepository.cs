using OrdersPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Domain.Repositories
{
	public interface IOrderPartsReasonRepository : IRepository<OrderPartsReason>
	{
		IList<OrderPartsReason> GetList();
	}
}
