using System.Collections.Generic;
using System.Linq;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.Infrastructure.Repositories
{
	public class RegionRepository : Repository<Region>, IRegionRepository
	{
		public RegionRepository(UnitOfWork unitOfWork) : base(unitOfWork)
		{

		}
		public List<Region> GetList()
		{
			return DbSet.ToList();
		}
	
	}
}
