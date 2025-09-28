using System.Collections.Generic;
using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Domain.Repositories
{
	public interface IRegionRepository : IRepository<Region>
	{
		List<Region> GetList();

	}
}
