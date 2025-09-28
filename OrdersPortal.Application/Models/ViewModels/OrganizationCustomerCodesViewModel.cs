using OrdersPortal.Domain.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class OrganizationCustomerCodesViewModel
	{
		[Display(Name = "Код Контрагента")]
		public int CustomerCode { get; set; }
		public List<CustomerCodesListDto> ContrCodesList { get; set; }
	}
}
