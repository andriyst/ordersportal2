using System.Linq;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.Infrastructure.Repositories
{

	public class HelpServiceContactRepository : Repository<HelpServiceContact>, IHelpServiceContactRepository
	{
		public HelpServiceContactRepository(UnitOfWork unitOfWork) : base(unitOfWork)
		{

		}

		public HelpServiceContact GetContactByType(HelpServiceTypesEnum helpServiceType)
		{
			return DbSet.FirstOrDefault(x => x.HelpServiceType == helpServiceType);
		}
	}
}
