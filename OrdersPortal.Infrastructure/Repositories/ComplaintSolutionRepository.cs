using System.Collections.Generic;
using System.Linq;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.Infrastructure.Repositories
{
	
	public class ComplaintSolutionRepository : Repository<ComplaintSolution>, IComplaintSolutionRepository
	{
		public ComplaintSolutionRepository(UnitOfWork unitOfWork) : base(unitOfWork)
		{

		}

		public List<ComplaintSolution> GetComplaintSolutionByIssueId(int issueId)
		{
			return DbSet.Where(x => x.ComplaintIssueId == issueId && x.Enabled).ToList();
		}
	}

}
