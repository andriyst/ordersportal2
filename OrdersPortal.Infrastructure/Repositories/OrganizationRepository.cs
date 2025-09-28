using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;


namespace OrdersPortal.Infrastructure.Repositories
{
	public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
	{
		public OrganizationRepository(UnitOfWork unitOfWork) : base(unitOfWork)
		{

		}
		public List<Organization> GetList()
		{
			return DbSet.ToList();
		}
	}
}
