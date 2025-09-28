using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Domain.Dto
{
	public class Update1cManagersDto
	{

		[Display(Name = "Оновлення менеджерів")]
		public Boolean Managers { get; set; }
		[Display(Name = "Оновлення Регіональних менеджерів")]
		public Boolean RegionManagers { get; set; }
		public List<Customer> UpdatedCustomers { get; set; }

		[Display(Name = "Організація")]
		public string Organization1cId { get; set; }
		public List<Organization> OrganizationList { get; set; }
	}
}