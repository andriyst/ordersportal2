using OrdersPortal.Domain.Dto;
using OrdersPortal.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Application.Models
{
	public class RegionManagersViewModel
	{
		[Display(Name = "Рег. Менеджер")]
		public string RegionManagerId { get; set; }
		public virtual List<OrderPortalUser> RegionManagerList { get; set; }

	}
}
