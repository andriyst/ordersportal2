using OrdersPortal.Domain.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Application.Models
{
	public class RegionManagers1cGuidsViewModel
	{	
	
		[Display(Name = "Рег. Менеджер Guid")]
		public string RegionManagerGuid { get; set; }

		public List<RegionManager1c> RegionManagers1cList { get; set; }
	}
}
