using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class AnalyticsBaseViewModel
	{
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
		public DateTime StartDate { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
		public DateTime EndDate { get; set; }

		public string ContrAgentCode { get; set; }

		public List<SelectListItem> ContrAgentCodeList { get; set; }
	}
}
