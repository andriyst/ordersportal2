using System.Collections.Generic;
using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Domain.Repositories
{
	public interface IOrganizationRepository : IRepository<Organization>
	{
		List<Organization> GetList();

	}
}
