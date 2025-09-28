using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Application.Services
{
	public interface IHelpServiceService
	{
		void SaveHelpServiceContacts(HelpServiceConfigureViewModel model);
		HelpServiceConfigureViewModel GetHelpServiceContacts();
		bool CallManager(HelpServiceTypesEnum helpServiceType);
		HelpServiceStatsViewModel GetHelpServiceStats(string startDate, string endDate);
	}
}
