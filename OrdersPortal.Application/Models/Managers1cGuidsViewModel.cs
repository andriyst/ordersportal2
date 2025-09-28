using OrdersPortal.Domain.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Application.Models
{
	public class Managers1cGuidsViewModel
	{
	
		[Display(Name = "Менеджер Guid")]
		public string ManagerGuid { get; set; }


		public List<Manager1c> Managers1cList { get; set; }
	}
}
