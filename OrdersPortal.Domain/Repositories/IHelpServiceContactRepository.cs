
using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Domain.Repositories
{
	public interface IHelpServiceContactRepository : IRepository<HelpServiceContact>
	{

		HelpServiceContact GetContactByType(HelpServiceTypesEnum helpServiceType);
	}
}
