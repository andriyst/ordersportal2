using OrdersPortal.Domain.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class OrganizationAddViewModel
	{
		[Required]
		[Display(Name = "Назва Організації")]
		public string OrganizationName { get; set; }

		[Required]
		[Display(Name = "Назва Організації в 1С")]
		public string Organization1cId { get; set; }

		public List<Organization1c> Organization1cList { get; set; }
	}
}
