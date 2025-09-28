using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.Infrastructure.Repositories
{
	public class HelpServiceLogRepository : Repository<HelpServiceLog>, IHelpServiceLogRepository
	{
		public HelpServiceLogRepository(UnitOfWork unitOfWork) : base(unitOfWork)
		{

		}

		public List<HelpServiceLog> GetByPeriodDesc(DateTime startDate, DateTime endDate)
		{
			return DbSet.Include(x => x.OrderPortalUser)
			            .Include(x => x.HelpServiceContact)
			            .Where(x => x.CreateDate >= startDate && x.CreateDate <= endDate)
			            .OrderByDescending(x => x.CreateDate)
			            .ToList();
		}
	}
}
