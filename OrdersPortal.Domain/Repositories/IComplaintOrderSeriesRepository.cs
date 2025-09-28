using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Domain.Repositories
{
	public interface IComplaintOrderSeriesRepository : IRepository<ComplaintOrderSerie>
	{
		void RemoveByComplaintId(int complaintId);
	}
}
