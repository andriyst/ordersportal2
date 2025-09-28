using System.Linq;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.Infrastructure.Repositories
{
	public class StatusRepository : Repository<Status>, IStatusRepository
	{
		public StatusRepository(UnitOfWork unitOfWork) : base(unitOfWork)
		{

		}

		public Status GetStatusByName(string statusName)
		{
			return DbSet.FirstOrDefault(x => x.StatusName == statusName);
		}

	}
}
