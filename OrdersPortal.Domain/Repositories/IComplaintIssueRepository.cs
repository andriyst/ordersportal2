using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Domain.Repositories
{
	public interface IComplaintIssueRepository : IRepository<ComplaintIssue>
	{
		ComplaintIssue GetByGuid(string guid);
	}
}
