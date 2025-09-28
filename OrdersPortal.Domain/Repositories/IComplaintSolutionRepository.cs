using System.Collections.Generic;

using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Domain.Repositories
{
	public interface IComplaintSolutionRepository : IRepository<ComplaintSolution>
	{
		List<ComplaintSolution> GetComplaintSolutionByIssueId(int issueId);
	}
}
