using OrdersPortal.Domain.Dto;
using OrdersPortal.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Application.Models
{
	public class ManagersViewModel
	{

		[Display(Name = "Менеджер")]
		public string ManagerId { get; set; }
		public virtual List<OrderPortalUser> ManagerList { get; set; }

	}
}
