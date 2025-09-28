using System;
using System.Collections.Generic;
using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Domain.Repositories
{
	public interface IHelpServiceLogRepository : IRepository<HelpServiceLog>
	{
		List<HelpServiceLog> GetByPeriodDesc(DateTime startDate, DateTime endDate);

	}
}
