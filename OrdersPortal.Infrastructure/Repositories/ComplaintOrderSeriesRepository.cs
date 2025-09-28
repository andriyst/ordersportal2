using System.Linq;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.Infrastructure.Repositories
{
	public class ComplaintOrderSeriesRepository : Repository<ComplaintOrderSerie>, IComplaintOrderSeriesRepository
	{
		public ComplaintOrderSeriesRepository(UnitOfWork unitOfWork) : base(unitOfWork)
		{

		}

		public void RemoveByComplaintId(int complaintId)
		{
			DbSet.RemoveRange(DbSet.Where(x => x.ComplaintId == complaintId));
		}
	}
}
