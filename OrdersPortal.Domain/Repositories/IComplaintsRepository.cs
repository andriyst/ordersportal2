using System.Collections.Generic;
using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Domain.Repositories
{
	public interface IComplaintsRepository : IRepository<Complaint>
	{
		List<Complaint> GetList();
		Complaint GetByIdWithIncludes(int complaintId);
		Complaint GetByIdWithDecision(int complaintId);

	}
}
