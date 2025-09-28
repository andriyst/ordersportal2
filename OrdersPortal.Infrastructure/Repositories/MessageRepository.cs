using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.Infrastructure.Repositories
{
	public class MessageRepository : Repository<OrdersMessage>, IMessageRepository
	{
		public MessageRepository(UnitOfWork unitOfWork) : base(unitOfWork)
		{

		}

		public OrdersMessage GetLastOrderMessage(int orderId)
		{
			return DbSet.AsNoTracking()
								.Include(x => x.MessageWriter)
								.Where(x => x.OrderId == orderId)
								.OrderByDescending(x => x.MessageTime)
								.FirstOrDefault();
		}

		public List<OrdersMessage> GetOrderMessages(int orderId)
		{
			return DbSet.Include(y => y.MessageWriter.Roles).Where(x => x.OrderId == orderId).OrderByDescending(x=>x.MessageTime).ToList();
		}
	}
}
