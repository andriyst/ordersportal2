using System;
using System.Linq;
using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.Application.Services
{
	public class StatService: IStatService
	{
		private readonly IHelpServiceLogRepository _helpServiceLogRepository;
		public StatService(IHelpServiceLogRepository helpServiceLogRepository)
		{
			_helpServiceLogRepository = helpServiceLogRepository;
		}

		public HelpServiceStatsViewModel GetHelpServiceStats(string startDate, string endDate)
		{


			HelpServiceStatsViewModel model = new HelpServiceStatsViewModel();

			if (string.IsNullOrEmpty(startDate))
			{
				model.StartDate = _helpServiceLogRepository.GetAll().OrderBy(x => x.CreateDate).FirstOrDefault()?.CreateDate ?? DateTime.Today;
			}
			else
			{
				model.StartDate = Convert.ToDateTime(startDate);
			}
			if (string.IsNullOrEmpty(endDate))
			{
				model.EndDate = DateTime.Now;
			}
			else
			{
				model.EndDate = Convert.ToDateTime(endDate).AddDays(1).AddMinutes(-1);
			}

			model.HelpServiceLogs = _helpServiceLogRepository.GetByPeriodDesc(model.StartDate, model.EndDate);
			
			return model;
		}
	}


}
