using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OrdersPortal.Domain.Dto.Customer1cOrder;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class CustomerBillViewModel : CustomerBillListDto
	{
		public string Guid { get; set; }
		public string Number { get; set; }
		public decimal Suma { get; set; }
		public DateTime Date { get; set; }
	}
}
