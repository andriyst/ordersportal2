using OrdersPortal.Application.Models.ViewModels;

namespace OrdersPortal.Application.Services
{
	public interface IStatService
	{
		HelpServiceStatsViewModel GetHelpServiceStats(string startDate, string endDate);
	}
}
