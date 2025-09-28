using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Domain.Repositories
{
	public interface IStatusRepository : IRepository<Status>
	{
		Status GetStatusByName(string statusName);
	}
}
