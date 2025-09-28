using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

using OrdersPortal.Domain.Dto;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class WdsActionCodeViewModel
	{
		public List<WdsActionCodeListModel> WdsActionCodeList { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
		public DateTime DateStart { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
		public DateTime DateEnd { get; set; }
		public string ContrCode { get; set; }

		public List<SelectListItem> ContrAgentCodeList { get; set; }
	}
}
