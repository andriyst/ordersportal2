using System.Linq;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.Infrastructure.Repositories
{

	public class ComplaintIssueRepository : Repository<ComplaintIssue>, IComplaintIssueRepository
	{
		public ComplaintIssueRepository(UnitOfWork unitOfWork) : base(unitOfWork)
		{
		}

		public ComplaintIssue GetByGuid(string guid)
		{
			return DbSet.FirstOrDefault(x => x.IssueGuid == guid);
		}

	}
}
